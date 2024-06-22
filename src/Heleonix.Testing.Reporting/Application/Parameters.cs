// <copyright file="Parameters.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application;

using Heleonix.Testing.Reporting.Domain;

/// <summary>
/// Parameters to be used for report generation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Parameters"/> class.
/// </remarks>
public class Parameters
{
    /// <summary>
    /// Gets the list of input files to generate reports from.
    /// </summary>
    public FileInfo[] Input { get; init; }

    /// <summary>
    /// Gets the <see cref="FileSystemInfo"/> representing an output path for the generated report: a file or a directory.
    /// </summary>
    public FileSystemInfo Output { get; init; }

    /// <summary>
    /// Gets the list of formats to generate repors in from the list of supported <see cref="ReportFormat"/>.
    /// </summary>
    public ReportFormat[] Formats { get; init; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Input"/> files should be merged into
    /// a single <see cref="Output"/> report or every input file should have it's own generated output report.
    /// </summary>
    public bool Merge { get; init; }

    /// <summary>
    /// Gets the list of custom values for the supported CSS variables to apply to the generated report
    /// to override default styles.
    /// </summary>
    public IDictionary<string, string> Styles { get; init; }

    /// <summary>
    /// Gets the list of custom values for the ObjectModel to override the default content
    /// in the generated report.
    /// </summary>
    public IDictionary<string, string> Content { get; init; }
}
