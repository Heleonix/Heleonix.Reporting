// <copyright file="TrxTestResultLoader.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application.Loaders.Trx;

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Heleonix.Testing.Reporting.Application.Loaders;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;

/// <summary>
/// Implements loading of test results in <c>trx</c> format and converts to the unified <see cref="TestResult"/> model.
/// </summary>
/// <param name="streamProvider">The provider of streams to read test results from.</param>
/// <param name="logger">The logger to log the porcess of reports generation.</param>
public class TrxTestResultLoader(
    IStreamProvider streamProvider,
    ILogger<TrxTestResultLoader> logger) : ITestResultLoader
{
    private static readonly Regex NamespaceRegExp = new(@"[\w\d_.]+\.", RegexOptions.Compiled);

    private static readonly Regex ClassNameFromFullNameRegExp = new(@"^[^,]+", RegexOptions.Compiled);

    private static readonly Dictionary<string, Outcome> OutcomeMapping = new()
    {
        { "Error", Outcome.Error },
        { "Failed", Outcome.Failed },
        { "Warning", Outcome.Warning },
        { "Passed", Outcome.Passed },
        { "Inconclusive", Outcome.Skipped },
    };

    /// <summary>
    /// Loads test results in <c>trx</c> format from the specified <paramref name="filePath"/>
    /// and converts to the unified <see cref="TestResult"/> model.
    /// </summary>
    /// <param name="filePath">The path to the file to load xml-based test results from.</param>
    /// <returns>
    /// The loaded <c>trx</c> test results and converted into the unified <see cref="TestResult"/> model.
    /// </returns>
    /// <exception cref="NotImplementedException">Later.</exception>
    public TestResult Load(string filePath)
    {
        try
        {
            logger.LogInformation("Trying to load a test result from '{File}'", filePath);

            logger.LogDebug("Parsing the test result as TRX content in XML format");

            XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

            logger.LogDebug("Supported schema of the TRX file: {NS}.", ns);

            var result = new TestResult();

            using var stream = streamProvider.OpenInputStream(filePath);

            var doc = XDocument.Load(stream);

            result.Summary.Title = doc.Element(ns + "TestRun").Attribute("name").Value;

            result.Summary.Owner = doc.Element(ns + "TestRun").Attribute("runUser").Value;

            result.Summary.EndTime = Utc(doc.Descendants(ns + "Times").First().Attribute("finish").Value);

            var counters = doc.Descendants(ns + "Counters").First();
            var error = Convert.ToInt32(counters.Attribute("error").Value, CultureInfo.InvariantCulture);
            var failed = Convert.ToInt32(counters.Attribute("failed").Value, CultureInfo.InvariantCulture);
            var warning = Convert.ToInt32(counters.Attribute("warning").Value, CultureInfo.InvariantCulture);
            var passed = Convert.ToInt32(counters.Attribute("passed").Value, CultureInfo.InvariantCulture);
            var skipped = Convert.ToInt32(counters.Attribute("inconclusive").Value, CultureInfo.InvariantCulture);
            var orderedOutcomes = new[]
            {
                (error, Outcome.Error),
                (failed, Outcome.Failed),
                (warning, Outcome.Warning),
                (passed, Outcome.Passed),
                (skipped, Outcome.Skipped),
                (1, Outcome.Other),
            };

            result.Summary.Outcome = orderedOutcomes.First(o => o.Item1 > 0).Item2;

            var assemblies = new List<TestItem>();
            var classes = new List<TestItem>();
            var testCases = new List<TestCase>();

            var tests = doc.Descendants(ns + "UnitTest");

            foreach (var testResult in doc.Descendants(ns + "UnitTestResult"))
            {
                var test = tests.First(tr => tr.Attribute("id").Value == testResult.Attribute("testId").Value);

                var assemblyTitle = Path.GetFileNameWithoutExtension(
                    test.Element(ns + "TestMethod").Attribute("codeBase").Value);

                if (!assemblies.Exists(a => a.Title.Equals(assemblyTitle, StringComparison.OrdinalIgnoreCase)))
                {
                    assemblies.Add(new TestItem
                    {
                        Title = assemblyTitle,
                    });
                }

                var className = ClassNameFromFullNameRegExp
                    .Match(test.Element(ns + "TestMethod").Attribute("className").Value).Value;

                className = NamespaceRegExp.Replace(className, string.Empty);

                if (!classes.Exists(c => c.Title.Equals(className, StringComparison.OrdinalIgnoreCase)))
                {
                    classes.Add(new TestItem
                    {
                        Title = className,
                        ParentKey = assemblies.First(a => a.Title.Equals(assemblyTitle, StringComparison.OrdinalIgnoreCase)).Key,
                    });
                }

                testCases.Add(new TestCase
                {
                    Title = testResult.Attribute("testName").Value,
                    ParentKey = classes.First(c => c.Title.Equals(className, StringComparison.OrdinalIgnoreCase)).Key,
                    Start = Utc(testResult.Attribute("startTime").Value),
                    End = Utc(testResult.Attribute("endTime").Value),
                    Duration = (int)TimeSpan.Parse(testResult.Attribute("duration").Value, CultureInfo.InvariantCulture).TotalMilliseconds,
                    Outcome = MapOutcome(testResult.Attribute("outcome").Value),
                    Output = testResult.Element(ns + "Output")?.Element(ns + "StdOut")?.Value ?? string.Empty,
                    Errors = (testResult.Element(ns + "Output")?.Element(ns + "ErrorInfo")?.Element(ns + "Message")?.Value
                        + Environment.NewLine + Environment.NewLine
                        + testResult.Element(ns + "Output")?.Element(ns + "ErrorInfo")?.Element(ns + "StackTrace")?.Value)
                        .Trim(),
                });
            }

            result.Assemblies = assemblies;
            result.Classes = classes;
            result.TestCases = testCases;

            return result;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, "Could not load the test result.");

            return null;
        }
    }

    private static DateTime Utc(string dt) => DateTime.Parse(dt, CultureInfo.InvariantCulture).ToUniversalTime();

    private static Outcome MapOutcome(string outcome) =>
        OutcomeMapping.ContainsKey(outcome) ? OutcomeMapping[outcome] : Outcome.Other;
}
