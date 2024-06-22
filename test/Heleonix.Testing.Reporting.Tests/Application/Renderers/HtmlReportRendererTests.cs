// <copyright file="HtmlReportRendererTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Application.Renderers;

using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Application.Renderers;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Text;

/// <summary>
/// Tests the <see cref="HtmlReportRenderer"/>.
/// </summary>
[ComponentTest(Type = typeof(HtmlReportRenderer))]
internal class HtmlReportRendererTests
{
    /// <summary>
    /// Tests the <see cref="HtmlReportRenderer.Render"/>.
    /// </summary>
    [MemberTest(Name = nameof(HtmlReportRenderer.Render))]
    public void Render()
    {
        MemoryStream outputStream = null;
        IStreamProvider streamProvider = null;
        IReportTemplateProvider reportTemplateProvider = null;
        ILogger<HtmlReportRenderer> logger = null;

        HtmlReportRenderer htmlReportRenderer = null;

        Report report = null;
        string filePath = null;

        Exception exception = null;

        When("the method is called", () =>
        {
            Arrange(() =>
            {
                filePath = @"X:\some\report\path.html";
                outputStream = new MemoryStream();
                streamProvider = Mock.Of<IStreamProvider>(sp => sp.OpenOutputStream(filePath) == outputStream);
                logger = Mock.Of<ILogger<HtmlReportRenderer>>();

                report = new Report
                {
                    Result = new TestResult
                    {
                        Summary = new Summary
                        {
                            Title = "Title 1",
                            Outcome = Outcome.Passed,
                            Owner = "Owner 1",
                            EndTime = DateTime.Now,
                        },
                        Assemblies = new[] { new TestItem { ParentKey = Guid.NewGuid(), Title = "Assembly 1" } },
                        Classes = new[] { new TestItem { ParentKey = Guid.NewGuid(), Title = "Class 1" } },
                        TestCases = new[] { new TestCase { Title = "Test Case 1", Outcome = Outcome.Passed } },
                    },
                    Styles = new Dictionary<string, string>(),
                };
            });

            Act(() =>
            {
                try
                {
                    htmlReportRenderer = new HtmlReportRenderer(streamProvider, reportTemplateProvider, logger);

                    htmlReportRenderer.Render(report, filePath);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });

            Teardown(() =>
            {
                outputStream.Dispose();
            });

            And("the CSHTML model is valid", () =>
            {
                Arrange(() =>
                {
                    reportTemplateProvider = Mock.Of<IReportTemplateProvider>(
                        rtp => rtp.GetTemplate(ReportFormat.Html) == @"@Model.Result.Summary.Title");
                });

                Should("successfully render the report using the provided CSHTML template", () =>
                {
                    Assert.DoesNotThrow(() => Mock.Get(streamProvider).Verify(p => p.OpenOutputStream(filePath)));

                    var result = Encoding.ASCII.GetString(outputStream.ToArray());

                    Assert.That(result, Is.EqualTo("Title 1"));
                });
            });

            And("the CSHTML model is invalid", () =>
            {
                Arrange(() =>
                {
                    reportTemplateProvider = Mock.Of<IReportTemplateProvider>(
                        rtp => rtp.GetTemplate(ReportFormat.Html) == @"@Mo del.Re sult.Su mmary.Ti tle");
                });

                Should("fail rendering with the re-thrown exception", () =>
                {
                    Assert.DoesNotThrow(() =>
                    {
                        Mock.Get(streamProvider).Verify(p => p.OpenOutputStream(It.IsAny<string>()), Times.Never);
                    });

                    Assert.That(exception, Is.InstanceOf<Exception>());
                });
            });
        });
    }
}
