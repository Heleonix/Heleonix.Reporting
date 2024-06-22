// <copyright file="TestItem.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Domain;

using System;

/// <summary>
/// Represents the test item in the report, i.e. test assembly, or test class with multiple test cases.
/// </summary>
public class TestItem
{
    /// <summary>
    /// Gets the unique key of this item to use for filtering its children in reports.
    /// </summary>
    public Guid Key { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the unique key of parent of this item to be filtered by.
    /// Can be <c>null</c> if this item is a root parent, i.e. a test assembly.
    /// </summary>
    public Guid? ParentKey { get; set; }

    /// <summary>
    /// Gets or sets the title to be displayed in the report, i.e. assembly name of class name or test case summary.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}
