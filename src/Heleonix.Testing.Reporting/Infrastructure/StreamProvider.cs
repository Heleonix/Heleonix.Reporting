// <copyright file="StreamProvider.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Infrastructure;

using Heleonix.Testing.Reporting.Application;

/// <summary>
/// Implements functionality to provide sttreams for reading of input test results or writing human friendly reports.
/// </summary>
public class StreamProvider : IStreamProvider
{
    /// <inheritdoc/>
    public Stream OpenInputStream(string input) => File.OpenRead(input);

    /// <inheritdoc/>
    public Stream OpenOutputStream(string output)
    {
        if (!Directory.Exists(Path.GetDirectoryName(output)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(output));
        }

        return File.Create(output);
    }
}
