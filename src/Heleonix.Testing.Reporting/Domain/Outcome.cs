// <copyright file="Outcome.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Domain;

/// <summary>
/// Defines possible test execution outcomes.
/// </summary>
public enum Outcome
{
    /// <summary>
    /// Represents the test, which has an unknown or not yet supported status after test execution.
    /// </summary>
    Other = 0,

    /// <summary>
    /// Represents the test, which was skipped during test execution.
    /// </summary>
    Skipped = 1,

    /// <summary>
    /// Represents successfully passed test or test report.
    /// </summary>
    Passed = 2,

    /// <summary>
    /// represents test outcome or the test report outcome with warnings.
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Represents successfully executed, but failed test or the whole test report.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Represents an error in test execution or the whole test report execution.
    /// </summary>
    Error = 5,
}
