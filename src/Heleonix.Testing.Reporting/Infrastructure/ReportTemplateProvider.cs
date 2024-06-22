// <copyright file="ReportTemplateProvider.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Infrastructure;

using System.Text;
using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Domain;

/// <inheritdoc/>
public class ReportTemplateProvider : IReportTemplateProvider
{
    /// <inheritdoc/>
    public string GetTemplate(ReportFormat format)
    {
        var cshtml = new StringBuilder(Resources.Html);

        cshtml
            .Replace("@*CssTemplate*@", Resources.Css)
            .Replace("@*JsTemplate*@", Resources.Js);

        return cshtml.ToString();
    }
}
