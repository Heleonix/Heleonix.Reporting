// <copyright file="ReportTemplateProviderTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Infrastructure;

using Heleonix.Testing.Reporting.Domain;
using Heleonix.Testing.Reporting.Infrastructure;

/// <summary>
/// Tests the <see cref="ReportTemplateProvider"/>.
/// </summary>
[ComponentTest(Type = typeof(ReportTemplateProvider))]
internal class ReportTemplateProviderTests
{
    /// <summary>
    /// Tests the <see cref="ReportTemplateProvider.GetTemplate"/>.
    /// </summary>
    [MemberTest(Name = nameof(ReportTemplateProvider.GetTemplate))]
    public void GetTemplate()
    {
        ReportTemplateProvider provider = null;

        var format = default(ReportFormat);

        string result = null;

        When("the method is called", () =>
        {
            Act(() =>
            {
                provider = new ReportTemplateProvider();

                result = provider.GetTemplate(format);
            });

            And("the output format is 'Html'", () =>
            {
                Arrange(() =>
                {
                    format = ReportFormat.Html;
                });

                Should("return the embedded CSHTML template with JavaScript and CSS", () =>
                {
                    Assert.That(result, Contains.Substring("document.querySelectorAll"));
                    Assert.That(result, Contains.Substring("--color-primary"));
                    Assert.That(result, Contains.Substring("<body class=\"main-page\">"));
                });
            });
        });
    }
}
