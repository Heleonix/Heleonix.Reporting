// <copyright file="SuccessfulTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Samples;

/// <summary>
/// Simulates successful tests.
/// </summary>
// [ComponentTest(Type = typeof(SuccessfulTests))]
[Ignore("Dev time.")]
internal class SuccessfulTests
{
    /// <summary>
    /// Simulates a successfull test with output.
    /// </summary>
    [MemberTest(Name = nameof(SuccessfulTest1))]
    public void SuccessfulTest1()
    {
        When("the method is called", () =>
        {
            Should("succeed", () =>
            {
                Assert.IsTrue(1 == 1);
            });
        });
    }

    /// <summary>
    /// Simulates the successful test with no output.
    /// </summary>
    [Test]
    public void SuccessfulTest2()
    {
        Assert.IsTrue(1 == 1);
    }
}
