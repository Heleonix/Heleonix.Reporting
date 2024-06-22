// <copyright file="WarningTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Samples;

/// <summary>
/// Simulates successful tests.
/// </summary>
// [ComponentTest(Type = typeof(WarningTests))]
[Ignore("Dev time.")]
internal class WarningTests
{
    /// <summary>
    /// Simulates a test with warning.
    /// </summary>
    [Test]
    public void WarningTest1()
    {
        Assert.Warn("Warning message.");
    }
}
