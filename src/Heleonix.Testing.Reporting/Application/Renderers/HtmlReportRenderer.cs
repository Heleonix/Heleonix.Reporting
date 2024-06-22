// <copyright file="HtmlReportRenderer.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application.Renderers;

using System;
using System.IO;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;
using RazorEngineCore;

/// <summary>
/// Implements generation of a human friendly report as a standalone HTML.
/// </summary>
/// <param name="streamProvider">The provider of streams to read test results from.</param>
/// <param name="templateProvider">The provider of templates for renderers to generate reports.</param>
/// <param name="logger">The logger to log the porcess of reports rendering.</param>
public class HtmlReportRenderer(
    IStreamProvider streamProvider,
    IReportTemplateProvider templateProvider,
    ILogger<HtmlReportRenderer> logger) : IReportRenderer
{
    private readonly Lazy<RazorEngine> engine = new();

    private IRazorEngineCompiledTemplate<RazorEngineTemplateBase<Report>> reportTemplate;

    /// <inheritdoc/>
    public ReportFormat SupportedFormat { get; } = ReportFormat.Html;

    /// <summary>
    /// Writes an HTML content from the provided <paramref name="report"/> into the provided <paramref name="filePath"/>.
    /// </summary>
    /// <param name="report">The unified report model to be written into the provided <paramref name="filePath"/>.</param>
    /// <param name="filePath">The path to the file to write an HTML report to.</param>
    public void Render(Report report, string filePath)
    {
        logger.LogInformation(
            "Rendering '{Title}' as '{Format}' into the '{File}'",
            report.Result.Summary.Title,
            ReportFormat.Html,
            filePath);

        var template = templateProvider.GetTemplate(ReportFormat.Html);

        try
        {
            this.reportTemplate ??= this.engine.Value.Compile<RazorEngineTemplateBase<Report>>(template);

            var result = this.reportTemplate.Run(instance => instance.Model = report);

            using var stream = streamProvider.OpenOutputStream(filePath);

            using (var textWriter = new StreamWriter(stream))
            {
                textWriter.Write(result);
            }
        }
#pragma warning disable S2139 // Exceptions should be either logged or rethrown but not both
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not render the report using the cshtml template.");

            throw;
        }
#pragma warning restore S2139 // Exceptions should be either logged or rethrown but not both
    }
}
