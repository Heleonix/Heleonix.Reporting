// <copyright file="Program.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting;

using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Application.Loaders;
using Heleonix.Testing.Reporting.Application.Loaders.Trx;
using Heleonix.Testing.Reporting.Application.Renderers;
using Heleonix.Testing.Reporting.Infrastructure;
using Heleonix.Testing.Reporting.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

/// <summary>
/// The entry point of the tool.
/// </summary>
/// <exclude />
[ExcludeFromCodeCoverage]
public static class Program
{
    /// <summary>
    /// The main entry point of the tool. Bootstraps the application with required services via dependency injection.
    /// </summary>
    /// <param name="args">The CLI arguments.</param>
    /// <returns>An exit code of the tool.</returns>
    public static int Main(string[] args)
    {
        var cliRootCommand = new CliRootCommand();

        var traceLevel = cliRootCommand.Parse(args).GetValueForOption(cliRootCommand.VerbosityOption);

        var services = new ServiceCollection();

        services
            .AddLogging(builder => builder
                .AddConsole(opt => opt.FormatterName = nameof(CliLoggingFormatter))
                .AddConsoleFormatter<CliLoggingFormatter, ConsoleFormatterOptions>()
                .SetMinimumLevel(traceLevel))
            .AddSingleton<IReportGenerator, ReportGenerator>()
            .AddSingleton<IStreamProvider, StreamProvider>()
            .AddSingleton<ITestResultLoader, TrxTestResultLoader>()
            .AddSingleton<ITestResultMerger, TestResultMerger>()
            .AddSingleton<IReportRenderer, HtmlReportRenderer>()
            .AddSingleton<IReportTemplateProvider, ReportTemplateProvider>();

        using var serviceProvider = services.BuildServiceProvider();

        var reportGenerator = serviceProvider.GetRequiredService<IReportGenerator>();

        cliRootCommand.Invoker = reportGenerator.GenerateReport;

        return cliRootCommand.Invoke(args);
    }
}