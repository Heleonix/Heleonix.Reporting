// <copyright file="ITestResultMerger.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application;

using Heleonix.Testing.Reporting.Domain;

/// <summary>
/// A merger to merge multiple test results loaded by loaders into a single test result.
/// </summary>
public interface ITestResultMerger
{
    /// <summary>
    /// Merges multiple test results loaded by loaders into the a single test result.
    /// </summary>
    /// <param name="results">The results loaded by loaders to merge.</param>
    /// <returns>The merged test result.</returns>
    TestResult Merge(IEnumerable<TestResult> results);
}
