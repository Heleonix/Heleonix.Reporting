// <copyright file="CliLoggingFormatter.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Presentation;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

/// <summary>
/// A custom logging formatter to write simple log messages.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CliLoggingFormatter"/> class.
/// </remarks>
public sealed class CliLoggingFormatter() : ConsoleFormatter(nameof(CliLoggingFormatter))
{
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Console/src/AnsiParser.cs
    private const string DefaultForegroundColorExcapeCode = "\x1B[39m\x1B[22m";

    private readonly Dictionary<LogLevel, string> logLevelToColorCode = new()
    {
        { LogLevel.Trace, "\x1B[37m" }, // Gray
        { LogLevel.Debug, "\x1B[37m" }, // Gray
        { LogLevel.Information, "\x1B[32m" }, // DarkGreen
        { LogLevel.Warning, "\x1B[1m\\x1B[33m" }, // Yellow
        { LogLevel.Error, "\x1B[31m" }, // DarkRed
        { LogLevel.Critical, "\x1B[31m" }, // DarkRed
    };

    /// <inheritdoc/>
    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
    {
        textWriter.Write(this.logLevelToColorCode[logEntry.LogLevel]);

        textWriter.WriteLine($"{logEntry.LogLevel}:");

        textWriter.Write(DefaultForegroundColorExcapeCode);

        textWriter.WriteLine($"\t{logEntry.Formatter(logEntry.State, logEntry.Exception)}");
    }
}