// <copyright file="TestResultMerger.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application;

using System.Collections.Generic;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;

/// <summary>
/// The implementation of the <see cref="ITestResultMerger"/> report merger.
/// </summary>
/// <param name="logger">The logger to log the porcess of reports generation.</param>
public class TestResultMerger(ILogger<TestResultMerger> logger) : ITestResultMerger
{
    /// <inheritdoc/>
    public TestResult Merge(IEnumerable<TestResult> results)
    {
        logger.LogInformation("Merging of {Count} loaded test results into a single test result", results.Count());

        var mergedResult = new TestResult();

        logger.LogDebug("Merging summaries");

        MergeSummary(results, mergedResult);

        mergedResult.Assemblies = results.SelectMany(r => r.Assemblies).ToArray();

        mergedResult.Classes = results.SelectMany(r => r.Classes).ToArray();

        mergedResult.TestCases = results.SelectMany(r => r.TestCases).ToArray();

        return mergedResult;
    }

    private static void MergeSummary(IEnumerable<TestResult> results, TestResult mergedResult)
    {
        mergedResult.Summary.Title = results
                    .Where(r => !string.IsNullOrEmpty(r.Summary.Title))
                    .Select(r => r.Summary.Title)
                    .FirstOrDefault();

        mergedResult.Summary.Owner = results
            .Where(r => !string.IsNullOrEmpty(r.Summary.Owner))
            .Select(r => r.Summary.Owner)
            .FirstOrDefault();

        mergedResult.Summary.Outcome = results.Max(r => r.Summary.Outcome);

        mergedResult.Summary.EndTime = results.Max(r => r.Summary.EndTime);
    }
}
