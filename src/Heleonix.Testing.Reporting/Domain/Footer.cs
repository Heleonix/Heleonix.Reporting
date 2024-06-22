// <copyright file="Footer.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Domain;

/// <summary>
/// Represents content of the footer to be rendered in the report.
/// </summary>
public class Footer
{
    /// <summary>
    /// Gets or sets the text to be displayed in the footer.
    /// </summary>
    public string Text { get; set; } = Resources.Footer_Text;

    /// <summary>
    /// Gets or sets the url the <see cref="Text"/> should lead to.
    /// </summary>
    public string Url { get; set; } = Resources.Footer_Url;
}
