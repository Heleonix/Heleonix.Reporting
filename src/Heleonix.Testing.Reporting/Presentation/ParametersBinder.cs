// <copyright file="ParametersBinder.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Presentation;

using System.CommandLine;
using System.CommandLine.Binding;
using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Domain;

/// <summary>
/// Binds the command line arguments to the application-level <see cref="Parameters"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ParametersBinder"/> class.
/// </remarks>
/// <param name="input">The list of input files to generate reports from.</param>
/// <param name="output">The string representing an output path for the generated report.</param>
/// <param name="formats">
/// The list of formats to generate repors in from the list of supported <see cref="ReportFormat"/>.
/// </param>
/// <param name="merge">Specifies whether the <paramref name="input"/> files should be merged into
/// a single <paramref name="output"/> report or every input file should have it's own generated output report.
/// </param>
/// <param name="styles">
/// The list of custom values for the supported CSS variables to apply to the generated report
/// to override default styles.
/// </param>
/// <param name="content">The list of custom values for the ObjectModel to override the default content
/// in the generated report.
/// </param>
public class ParametersBinder(
    Option<FileInfo[]> input,
    Option<FileSystemInfo> output,
    Option<ReportFormat[]> formats,
    Option<bool> merge,
    Option<string[]> styles,
    Option<string[]> content) : BinderBase<Parameters>
{
    /// <inheritdoc/>
    protected override Parameters GetBoundValue(BindingContext bindingContext) =>
        new Parameters
        {
            Input = bindingContext.ParseResult.GetValueForOption(input),
            Output = bindingContext.ParseResult.GetValueForOption(output),
            Formats = bindingContext.ParseResult.GetValueForOption(formats),
            Merge = bindingContext.ParseResult.GetValueForOption(merge),
            Styles = SplitOption(styles, bindingContext),
            Content = SplitOption(content, bindingContext),
        };

    private static IDictionary<string, string> SplitOption(Option<string[]> option, BindingContext bindingContext) =>
        new Dictionary<string, string>(bindingContext.ParseResult.GetValueForOption(option).Select(Splitter));

    private static KeyValuePair<string, string> Splitter(string option)
    {
        var values = option.Split("=");

        return new KeyValuePair<string, string>(values[0], values[1]);
    }
}
