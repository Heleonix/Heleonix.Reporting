// <copyright file="FailedTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Samples;

/// <summary>
/// Simulates failed tests.
/// </summary>
// [ComponentTest(Type = typeof(FailedTests))]
[Ignore("Dev time.")]
internal class FailedTests
{
    /// <summary>
    /// Simulates a failed test.
    /// </summary>
    [MemberTest(Name = nameof(FailedTest1))]
    public void FailedTest1()
    {
        When("the method is called", () =>
        {
            Should("fail", () =>
            {
                Assert.Fail();
            });
        });
    }

    /// <summary>
    /// Simulates a failed test with no output.
    /// </summary>
    [Test]
    public void FailedTest2()
    {
        Assert.Fail("Error message.");
    }
}
