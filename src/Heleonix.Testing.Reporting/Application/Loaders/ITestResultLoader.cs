// <copyright file="ITestResultLoader.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application.Loaders;

using Heleonix.Testing.Reporting.Domain;

/// <summary>
/// The common interface for concrete loaders to load test results in different formats, like 'trx', 'nunit' etc.
/// </summary>
public interface ITestResultLoader
{
    /// <summary>
    /// When implemented by a concrete loader, reads test results from <paramref name="filePath"/> and  converts
    /// into the common unified <see cref="TestResult"/> for further generation of human-friendly reports.
    /// </summary>
    /// <param name="filePath">The path to the file to read test results from.</param>
    /// <returns>The unified <see cref="TestResult"/> for further generation of human-friendly reports.</returns>
    TestResult Load(string filePath);
}
