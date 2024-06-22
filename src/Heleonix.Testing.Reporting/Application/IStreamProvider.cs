// <copyright file="IStreamProvider.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Application;

/// <summary>
/// Provides streams for reading of input test results and writing of human-friendly reports.
/// </summary>
public interface IStreamProvider
{
    /// <summary>
    /// Opens the input stream for reading.
    /// </summary>
    /// <param name="input">The input to open the stream for.</param>
    /// <returns>The opened stream for reading.</returns>
    Stream OpenInputStream(string input);

    /// <summary>
    /// Opens the output stream for writing.
    /// </summary>
    /// <param name="output">The output path to open the stream for or in.</param>
    /// <returns>The opened stream for writing.</returns>
    Stream OpenOutputStream(string output);
}
