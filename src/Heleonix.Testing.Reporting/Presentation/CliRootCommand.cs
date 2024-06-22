// <copyright file="CliRootCommand.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Presentation;

using System;
using System.CommandLine;
using System.IO;
using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents the <see cref="RootCommand"/> implementation for the
/// <see cref="Reporting"/> CLI, which receives parsed command line arguments and launches
/// generation of reports.
/// </summary>
public class CliRootCommand : RootCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliRootCommand"/> class.
    /// </summary>
    public CliRootCommand()
        : base(Resources.CLI_Description)
    {
        var inputOption = new Option<FileInfo[]>(["--input", "-i"], Resources.CLI_Input_Description)
        {
            IsRequired = true,
            AllowMultipleArgumentsPerToken = true,
            ArgumentHelpName = "path",
        };

        inputOption.AddValidator(result =>
        {
            foreach (var fi in result.GetValueOrDefault<FileInfo[]>())
            {
                if (!fi.Exists)
                {
                    result.ErrorMessage = string.Format(Resources.CLI_FileNotFound, fi.FullName);
                }
            }
        });

        var outputOption = new Option<FileSystemInfo>(["--output", "-o"], Resources.CLI_Output_Description)
        {
            IsRequired = true,
            ArgumentHelpName = "path",
        };

        var formatOption = new Option<ReportFormat[]>(["--format", "-f"], Resources.CLI_Format_Description)
        {
            IsRequired = true,
            ArgumentHelpName = string.Join(", ", Enum.GetNames<ReportFormat>()),
            AllowMultipleArgumentsPerToken = true,
        };

        var mergeOption = new Option<bool>(["--merge", "-m"], Resources.CLI_Megre_Description);

        var styleOption = new Option<string[]>(["--style", "-s"], Resources.CLI_Style_Description)
        {
            ArgumentHelpName = "variable-name=value",
            AllowMultipleArgumentsPerToken = true,
        };

        var contentOption = new Option<string[]>(["--content", "-c"], Resources.CLI_Content_Description)
        {
            ArgumentHelpName = "property.path=value",
            AllowMultipleArgumentsPerToken = true,
        };

        this.VerbosityOption = new Option<LogLevel>(
            ["--verbosity", "-v"],
            () => LogLevel.Information,
            Resources.CLI_Verbosity_Description);

        this.AddOption(inputOption);
        this.AddOption(outputOption);
        this.AddOption(formatOption);
        this.AddOption(mergeOption);
        this.AddOption(styleOption);
        this.AddOption(contentOption);
        this.AddOption(this.VerbosityOption);

        this.SetHandler(
            parameters => this.Invoker(parameters),
            new ParametersBinder(inputOption, outputOption, formatOption, mergeOption, styleOption, contentOption));
    }

    /// <summary>
    /// Gets the verbosity option configured for the command line to be used for logging of the execution process.
    /// </summary>
    public Option<LogLevel> VerbosityOption { get; private set; }

    /// <summary>
    /// Gets or sets the method to invoke this command with passed <see cref="Parameters"/>.
    /// </summary>
    public Action<Parameters> Invoker { get; set; } = _ => { };
}
