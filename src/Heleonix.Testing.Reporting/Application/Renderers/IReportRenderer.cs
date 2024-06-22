// <copyright file="IReportRenderer.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application.Renderers;

using Heleonix.Testing.Reporting.Domain;

/// <summary>
/// Represents the interface for implementation of concrete report renderers, like 'html' etc.
/// </summary>
public interface IReportRenderer
{
    /// <summary>
    /// Gets the <see cref="ReportFormat"/> value specifying the supported output format, which this instance can render.
    /// </summary>
    public ReportFormat SupportedFormat { get; }

    /// <summary>
    /// When implemented, writes a human friendly test report from the <paramref name="report"/>
    /// into the provided <paramref name="filePath"/> in a specific format.
    /// </summary>
    /// <param name="report">The unified report model to generate human friendly report from.</param>
    /// <param name="filePath">The path to the file to write the report in a specific format to.</param>
    void Render(Report report, string filePath);
}
