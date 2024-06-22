// <copyright file="ReportGeneratorTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Application;

using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Application.Loaders;
using Heleonix.Testing.Reporting.Application.Renderers;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;

/// <summary>
/// Tests the <see cref="ReportGenerator"/>.
/// </summary>
[ComponentTest(Type = typeof(ReportGenerator))]
public class ReportGeneratorTests
{
    /// <summary>
    /// Tests the <see cref="ReportGenerator.GenerateReport"/>.
    /// </summary>
    [MemberTest(Name = nameof(ReportGenerator.GenerateReport))]
    public void GenerateReport()
    {
        ITestResultLoader loader = null;
        ITestResultMerger merger = null;
        IReportRenderer renderer = null;
        ILogger<ReportGenerator> logger = null;

        ReportGenerator reportGenerator = null;

        Parameters parameters = null;

        When("the method is called", () =>
        {
            Arrange(() =>
            {
                merger = Mock.Of<ITestResultMerger>();
                renderer = Mock.Of<IReportRenderer>(r => r.SupportedFormat == ReportFormat.Html);
                logger = Mock.Of<ILogger<ReportGenerator>>();
            });

            Act(() =>
            {
                reportGenerator = new ReportGenerator(new[] { loader }, merger, new[] { renderer }, logger);

                reportGenerator.GenerateReport(parameters);
            });

            And("merging of test results is not specified", () =>
            {
                loader = Mock.Of<ITestResultLoader>(l => l.Load(It.IsAny<string>()) == new TestResult());

                Arrange(() =>
                {
                    parameters = new Parameters
                    {
                        Input = new[] { new FileInfo("X:/result1.trx"), new FileInfo("X:/result2.trx") },
                        Output = new FileInfo("X:/folder"),
                        Merge = false,
                        Formats = new[] { ReportFormat.Html },
                        Styles = new Dictionary<string, string>(),
                        Content = new Dictionary<string, string>
                        {
                            { "Footer.Text", "Some Text" },
                            { "Footer.Url", "https://url.com" },
                        },
                    };
                });

                Should("successfully generate separate reports for each test result", () =>
                {
                    Assert.DoesNotThrow(() =>
                    {
                        Mock.Get(loader).Verify(l => l.Load(parameters.Input[0].FullName), Times.Once);
                        Mock.Get(loader).Verify(l => l.Load(parameters.Input[1].FullName), Times.Once);
                        Mock.Get(merger).Verify(m => m.Merge(It.IsAny<IEnumerable<TestResult>>()), Times.Never);
                        Mock.Get(renderer).Verify(
                            r => r.Render(
                                It.Is<Report>(r => r.Footer.Text == "Some Text" && r.Footer.Url == "https://url.com"),
                                Path.Combine(
                                    parameters.Output.FullName,
                                    Path.ChangeExtension(Path.GetFileName(parameters.Input[0].FullName), "html"))),
                            Times.Once);
                        Mock.Get(renderer).Verify(
                            r => r.Render(
                                It.Is<Report>(r => r.Footer.Text == "Some Text" && r.Footer.Url == "https://url.com"),
                                Path.Combine(
                                    parameters.Output.FullName,
                                    Path.ChangeExtension(Path.GetFileName(parameters.Input[1].FullName), "html"))),
                            Times.Once);
                    });
                });
            });

            And("merging of test results is specified", () =>
            {
                loader = Mock.Of<ITestResultLoader>(l => l.Load(It.IsAny<string>()) == new TestResult());

                Arrange(() =>
                {
                    Mock.Get(merger).Setup(m => m.Merge(It.IsAny<IEnumerable<TestResult>>())).Returns(new TestResult());

                    parameters = new Parameters
                    {
                        Input = new[] { new FileInfo("X:/result1.trx"), new FileInfo("X:/result2.trx") },
                        Output = new FileInfo("X:/report"),
                        Merge = true,
                        Formats = new[] { ReportFormat.Html },
                        Styles = new Dictionary<string, string>(),
                        Content = new Dictionary<string, string>(),
                    };
                });

                Should("successfully generate the single report for all test results", () =>
                {
                    Assert.DoesNotThrow(() =>
                    {
                        Mock.Get(loader).Verify(l => l.Load(parameters.Input[0].FullName), Times.Once);
                        Mock.Get(loader).Verify(l => l.Load(parameters.Input[1].FullName), Times.Once);
                        Mock.Get(merger).Verify(m => m.Merge(It.IsAny<IEnumerable<TestResult>>()), Times.Once);
                        Mock.Get(renderer).Verify(
                            r => r.Render(It.IsAny<Report>(), Path.ChangeExtension(parameters.Output.FullName, "html")),
                            Times.Once);
                    });
                });
            });

            And("no test results loaded successfully", () =>
            {
                loader = Mock.Of<ITestResultLoader>(l => l.Load(It.IsAny<string>()) == null);

                Arrange(() =>
                {
                    parameters = new Parameters
                    {
                        Input = new[] { new FileInfo("X:/result1.trx") },
                        Output = new FileInfo("X:/report"),
                        Merge = false,
                        Formats = new[] { ReportFormat.Html },
                        Styles = new Dictionary<string, string>(),
                        Content = new Dictionary<string, string>(),
                    };
                });

                Should("successfully complete without rendering of reports", () =>
                {
                    Assert.DoesNotThrow(() =>
                    {
                        Mock.Get(loader).Verify(l => l.Load(parameters.Input[0].FullName), Times.Once);
                        Mock.Get(merger).Verify(m => m.Merge(It.IsAny<IEnumerable<TestResult>>()), Times.Never);
                        Mock.Get(renderer).Verify(r => r.Render(It.IsAny<Report>(), It.IsAny<string>()), Times.Never);
                    });
                });
            });
        });
    }
}
