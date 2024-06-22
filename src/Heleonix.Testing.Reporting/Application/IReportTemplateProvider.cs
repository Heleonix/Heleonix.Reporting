// <copyright file="IReportTemplateProvider.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application;

using Heleonix.Testing.Reporting.Domain;

/// <summary>
/// Represents an interface for getting templates for different renderers.
/// </summary>
public interface IReportTemplateProvider
{
    /// <summary>
    /// Gets the template for the specified report format.
    /// </summary>
    /// <param name="format">The report format to get a template for.</param>
    /// <returns>The template for the specified report format.</returns>
    string GetTemplate(ReportFormat format);
}
