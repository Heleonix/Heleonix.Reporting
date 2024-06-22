// <copyright file="PathHelper.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Samples;

/// <summary>
/// The helper to return paths to samples.
/// </summary>
internal static class PathHelper
{
    /// <summary>
    /// Gets the path to the 'Sample.trx'.
    /// </summary>
    public static string SampleTrx { get; } = Path.Combine(Environment.CurrentDirectory, nameof(Samples), "Sample.trx");
}
