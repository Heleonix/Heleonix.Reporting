// <copyright file="ReportGenerator.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application;

using Heleonix.Reflection;
using Heleonix.Testing.Reporting.Application.Loaders;
using Heleonix.Testing.Reporting.Application.Renderers;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;

/// <summary>
/// Implements generation of reports from the provided <see cref="Parameters"/>.
/// </summary>
/// <seealso cref="IReportGenerator" />
/// <remarks>
/// Initializes a new instance of the <see cref="ReportGenerator"/> class.
/// </remarks>
/// <param name="loaders">The loaders to load supported formats of test results, like <c>trx</c> etc.</param>
/// <param name="merger">The merger to merge multiple loaded test results into a single unified report model.</param>
/// <param name="renderers">The renderers to render the unified report model into output in a specified format.</param>
/// <param name="logger">The logger to log the porcess of reports generation.</param>
public class ReportGenerator(
    IEnumerable<ITestResultLoader> loaders,
    ITestResultMerger merger,
    IEnumerable<IReportRenderer> renderers,
    ILogger<ReportGenerator> logger) : IReportGenerator
{
    /// <inheritdoc/>
    public void GenerateReport(Parameters parameters)
    {
        logger.LogInformation("Starting reports generation");

        var results = new Dictionary<string, TestResult>();

        foreach (var input in parameters.Input.Select(i => i.FullName))
        {
            foreach (var loader in loaders)
            {
                var result = loader.Load(input);

                if (result != null)
                {
                    results.Add(input, result);

                    break;
                }
            }
        }

        if (parameters.Merge)
        {
            var mergedResults = merger.Merge(results.Values);

            results.Clear();

            results.Add(parameters.Output.FullName, mergedResults);
        }

        foreach (var result in results)
        {
            var report = new Report { Result = result.Value, Styles = parameters.Styles };

            logger.LogInformation("Applying custom content to the '{Title}' test result", result.Value.Summary.Title);

            foreach (var content in parameters.Content)
            {
                logger.LogDebug("Applying '{Path}' to '{Value}'", content.Key, content.Value);

                Reflector.SetCoerced(report, null, content.Key, content.Value);
            }

            foreach (var format in parameters.Formats)
            {
                var renderer = renderers.Single(r => r.SupportedFormat == format);

                string path;

                path = parameters.Merge
                    ? Path.ChangeExtension(result.Key, format.ToString().ToLower())
                    : Path.Combine(
                        parameters.Output.FullName,
                        Path.ChangeExtension(Path.GetFileName(result.Key), format.ToString().ToLower()));

                renderer.Render(report, path);
            }
        }

        logger.LogInformation("Success!");
    }
}
