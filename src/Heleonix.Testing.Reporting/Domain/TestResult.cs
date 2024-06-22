// <copyright file="TestResult.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Domain;

/// <summary>
/// The root model to represent the whole human friendly report to be generated in formats, like 'html' etc.
/// </summary>
public class TestResult
{
    /// <summary>
    /// Gets or sets content of summary to be rendered in the report.
    /// </summary>
    public Summary Summary { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of test assemblies to be displayed in the report.
    /// </summary>
    public IEnumerable<TestItem> Assemblies { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of test classes (aka fixtures) to be displayed in the report.
    /// </summary>
    public IEnumerable<TestItem> Classes { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of test cases to be displayed in the report.
    /// </summary>
    public IEnumerable<TestCase> TestCases { get; set; } = [];
}
