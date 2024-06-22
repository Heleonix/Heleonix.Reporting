// <copyright file="CliLoggingFormatterTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Presentation;

using Heleonix.Testing.Reporting.Presentation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

/// <summary>
/// Tests the <see cref="CliLoggingFormatter"/>.
/// </summary>
[ComponentTest(Type = typeof(CliLoggingFormatter))]
internal class CliLoggingFormatterTests
{
    /// <summary>
    /// Tests the <see cref="CliLoggingFormatter.Write"/>.
    /// </summary>
    [MemberTest(Name = nameof(CliLoggingFormatter.Write))]
    public void Write()
    {
        CliLoggingFormatter cliLoggingFormatter = null;

        LogEntry<int>? logEntry = null;
        StringWriter textWriter = null;

        When("the method is called", () =>
        {
            Arrange(() =>
            {
                logEntry = new (
                    LogLevel.Information,
                    string.Empty,
                    new EventId(1),
                    111,
                    new ArgumentException("Invalid argument"),
                    (i, ex) => $"{i} - {ex}");

                textWriter = new StringWriter();
            });

            Act(() =>
            {
                cliLoggingFormatter = new CliLoggingFormatter();

                cliLoggingFormatter.Write(logEntry.Value, null, textWriter);
            });

            Should("write the log using the specified formatter with the colored log level", () =>
            {
                var resutlt = textWriter.ToString();

                Assert.That(resutlt, Contains.Substring("\x1B[32m"));
                Assert.That(resutlt, Contains.Substring("Information:"));
                Assert.That(resutlt, Contains.Substring("Invalid argument"));
            });
        });
    }
}
