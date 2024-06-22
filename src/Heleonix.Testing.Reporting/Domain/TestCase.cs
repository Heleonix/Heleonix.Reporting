// <copyright file="TestCase.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Domain;

using System;

/// <summary>
/// Represents a test case in the report.
/// </summary>
public class TestCase : TestItem
{
    /// <summary>
    /// Gets or sets the outcome of the test case.
    /// </summary>
    public Outcome Outcome { get; set; }

    /// <summary>
    /// Gets or sets duration in milliseconds this test case executed.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the date/time in UTC when the tast case started execution.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the date/time in UTC when the tast case finished execution.
    /// </summary>
    public DateTime End { get; set; }

    /// <summary>
    /// Gets or sets the optional output of the test case, i.e. console output, logging etc.
    /// </summary>
    public string Output { get; set; }

    /// <summary>
    /// Gets or sets the optional errors of the test case, i.e. console error output, error logging etc.
    /// </summary>
    public string Errors { get; set; }
}
