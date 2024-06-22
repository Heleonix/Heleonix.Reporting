// <copyright file="IReportGenerator.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application;

/// <summary>
/// The entry point into report generation process using the provided parameters.
/// </summary>
public interface IReportGenerator
{
    /// <summary>
    /// Launches the process of report generation.
    /// </summary>
    /// <param name="parameters">The list of parameters to be used for report generation.
    /// </param>
    void GenerateReport(Parameters parameters);
}
