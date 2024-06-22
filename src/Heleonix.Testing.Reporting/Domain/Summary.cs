// <copyright file="Summary.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Domain;

/// <summary>
/// Represents content of the header to be rendered in the report.
/// </summary>
public class Summary
{
    /// <summary>
    /// Gets or sets the title of the report to be displayed depending on the output report type.
    /// For example the web page title in case of <c>html</c> reports.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the owner of the report to be displayed depending on the output report type.
    /// For example, PC/User of the machine generated input test results, company name etc.
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// Gets or sets the overall outcome of the test report.
    /// </summary>
    public Outcome Outcome { get; set; }

    /// <summary>
    /// Gets or sets the date/time when the whole test report has finished its execution.
    /// It corresponds to the latest date/time when the input test results were have been generated.
    /// </summary>
    public DateTime EndTime { get; set; }
}
