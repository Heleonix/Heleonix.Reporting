// <copyright file="Report.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Domain;

/// <summary>
/// The model to be passed into the renderer with <see cref="Report"/> data
/// and additional data for rendering of human friendly reports.
/// </summary>
public class Report
{
    /// <summary>
    /// Gets the report to be rendered.
    /// </summary>
    public TestResult Result { get; init; }

    /// <summary>
    /// Gets the footer's content to be rendered in the report.
    /// </summary>
    public Footer Footer { get; private set; } = new();

    /// <summary>
    /// Gets the custom styles to apply to the human friendly report.
    /// </summary>
    public IDictionary<string, string> Styles { get; init; }
}
