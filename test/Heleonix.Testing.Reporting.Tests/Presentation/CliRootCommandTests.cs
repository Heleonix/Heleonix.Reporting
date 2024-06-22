// <copyright file="CliRootCommandTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Presentation;

using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Domain;
using Heleonix.Testing.Reporting.Presentation;
using Heleonix.Testing.Reporting.Tests.Samples;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Parsing;

/// <summary>
/// Tests the <see cref="CliRootCommand"/>.
/// </summary>
[ComponentTest(Type = typeof(CliRootCommand))]
internal class CliRootCommandTests
{
    /// <summary>
    /// Tests the <see cref="CliRootCommand.CliRootCommand"/>.
    /// </summary>
    [MemberTest(Name = nameof(CliRootCommand))]
    public void CliRootCommand()
    {
        CliRootCommand command = null;

        string args = null;

        var exitCode = 0;

        When("the constructor is called", () =>
        {
            Act(() =>
            {
                command = new CliRootCommand();
            });

            Should("define all CLI options correctly", () =>
            {
                var inputOption = command.Options.First(o => o.Name == "input");
                var outputOption = command.Options.First(o => o.Name == "output");
                var formatOption = command.Options.First(o => o.Name == "format");
                var mergeOption = command.Options.First(o => o.Name == "merge");
                var styleOption = command.Options.First(o => o.Name == "style");
                var contentOption = command.Options.First(o => o.Name == "content");
                var verbosityOption = command.Options.First(o => o.Name == "verbosity");

                Assert.That(inputOption.Aliases.First(), Is.EqualTo("--input"));
                Assert.That(inputOption.Aliases.Last(), Is.EqualTo("-i"));
                Assert.That(inputOption.IsRequired, Is.True);
                Assert.That(inputOption.Description, Is.Not.Empty);

                Assert.That(outputOption.Aliases.First(), Is.EqualTo("--output"));
                Assert.That(outputOption.Aliases.Last(), Is.EqualTo("-o"));
                Assert.That(outputOption.IsRequired, Is.True);
                Assert.That(outputOption.Description, Is.Not.Empty);

                Assert.That(formatOption.Aliases.First(), Is.EqualTo("--format"));
                Assert.That(formatOption.Aliases.Last(), Is.EqualTo("-f"));
                Assert.That(formatOption.IsRequired, Is.True);
                Assert.That(formatOption.Description, Is.Not.Empty);

                Assert.That(mergeOption.Aliases.First(), Is.EqualTo("--merge"));
                Assert.That(mergeOption.Aliases.Last(), Is.EqualTo("-m"));
                Assert.That(mergeOption.IsRequired, Is.False);
                Assert.That(mergeOption.Description, Is.Not.Empty);

                Assert.That(styleOption.Aliases.First(), Is.EqualTo("--style"));
                Assert.That(styleOption.Aliases.Last(), Is.EqualTo("-s"));
                Assert.That(styleOption.IsRequired, Is.False);
                Assert.That(styleOption.Description, Is.Not.Empty);

                Assert.That(contentOption.Aliases.First(), Is.EqualTo("--content"));
                Assert.That(contentOption.Aliases.Last(), Is.EqualTo("-c"));
                Assert.That(contentOption.IsRequired, Is.False);
                Assert.That(contentOption.Description, Is.Not.Empty);

                Assert.That(verbosityOption.Aliases.First(), Is.EqualTo("--verbosity"));
                Assert.That(verbosityOption.Aliases.Last(), Is.EqualTo("-v"));
                Assert.That(verbosityOption.IsRequired, Is.False);
                Assert.That(verbosityOption.Description, Is.Not.Empty);
            });

            And("the default invoker is called", () =>
            {
                Should("do nothing", () =>
                {
                    Assert.DoesNotThrow(() => command.Invoker(null));
                });
            });

            And("the correct command line args are parsed", () =>
            {
                ParseResult result = null;

                Arrange(() =>
                {
                    args = $"-i {PathHelper.SampleTrx} -o ./Report.html -f Html";
                });

                Act(() =>
                {
                    result = command.Parse(args);
                });

                Should("provide parsed parameters", () =>
                {
                    Assert.That(result.GetValueForOption(command.VerbosityOption), Is.EqualTo(LogLevel.Information));
                });
            });

            And("the command is invoked with valid required command line arguments", () =>
            {
                Arrange(() =>
                {
                    args = $"-i {PathHelper.SampleTrx} -o ./Report.html -f Html";
                });

                Act(() =>
                {
                    exitCode = command.Invoke(args);
                });

                Should("be invoked successfully", () =>
                {
                    Assert.That(exitCode, Is.Zero);
                });
            });

            And("the command is invoked with valid optional command line arguments", () =>
            {
                Parameters parameters = null;

                Arrange(() =>
                {
                    args = $"-i {PathHelper.SampleTrx} -o ./Report.html -f Html -s color-bg-primary=#111 -c Content=Text";
                    command.Invoker = p => parameters = p;
                });

                Act(() =>
                {
                    exitCode = command.Invoke(args);
                });

                Should("be invoked successfully", () =>
                {
                    Assert.That(exitCode, Is.Zero);

                    Assert.That(parameters.Input.Single().FullName, Is.EqualTo(PathHelper.SampleTrx));

                    Assert.That(
                        parameters.Output.FullName,
                        Is.EqualTo($"{Environment.CurrentDirectory + Path.DirectorySeparatorChar}Report.html"));

                    Assert.That(parameters.Formats.Single(), Is.EqualTo(ReportFormat.Html));

                    Assert.That(parameters.Styles.Single().Key, Is.EqualTo("color-bg-primary"));
                    Assert.That(parameters.Styles.Single().Value, Is.EqualTo("#111"));

                    Assert.That(parameters.Content.Single().Key, Is.EqualTo("Content"));
                    Assert.That(parameters.Content.Single().Value, Is.EqualTo("Text"));
                });
            });

            And("the command is invoked with inexisting input test result file", () =>
            {
                Arrange(() =>
                {
                    args = $"-i ./NO_FILE.trx -o ./Report.html -f Html";
                });

                Act(() =>
                {
                    exitCode = command.Invoke(args);
                });

                Should("be invoked with an error", () =>
                {
                    Assert.That(exitCode, Is.Not.Zero);
                });
            });
        });
    }
}
