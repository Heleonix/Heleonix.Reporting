// <copyright file="TestResultMergerTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Application;

using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;

/// <summary>
/// Tests the <see cref="TestResultMerger"/>.
/// </summary>
[ComponentTest(Type = typeof(TestResultMerger))]
internal class TestResultMergerTests
{
    /// <summary>
    /// Tests the <see cref="TestResultMerger.Merge"/>.
    /// </summary>
    [MemberTest(Name = nameof(TestResultMerger.Merge))]
    public void Merge()
    {
        ILogger<TestResultMerger> logger = null;

        TestResultMerger testResultMerger = null;

        TestResult[] results = null;

        TestResult result = null;

        When("the method is called", () =>
        {
            Arrange(() =>
            {
                logger = Mock.Of<ILogger<TestResultMerger>>();

                results = new[]
                {
                    new TestResult
                    {
                        Summary = new Summary
                        {
                            Title = "Title 1",
                            Outcome = Outcome.Passed,
                            Owner = "Owner 1",
                            EndTime = DateTime.UtcNow.AddDays(-1),
                        },
                        Assemblies = new[] { new TestItem { ParentKey = Guid.NewGuid(), Title = "Assembly 1" } },
                        Classes = new[] { new TestItem { ParentKey = Guid.NewGuid(), Title = "Class 1" } },
                        TestCases = new[] { new TestCase { Title = "Test Case 1", Outcome = Outcome.Passed } },
                    },
                    new TestResult
                    {
                        Summary = new Summary
                        {
                            Title = "Title 2",
                            Outcome = Outcome.Failed,
                            Owner = "Owner 2",
                            EndTime = DateTime.UtcNow.AddDays(-2),
                        },
                        Assemblies = new[] { new TestItem { ParentKey = Guid.NewGuid(), Title = "Assembly 2" } },
                        Classes = new[] { new TestItem { ParentKey = Guid.NewGuid(), Title = "Class 2" } },
                        TestCases = new[] { new TestCase { Title = "Test Case 2", Outcome = Outcome.Failed } },
                    },
                };
            });

            Act(() =>
            {
                testResultMerger = new TestResultMerger(logger);

                result = testResultMerger.Merge(results);
            });

            Should("merge test results into a single result", () =>
            {
                Assert.That(result.Summary.Outcome, Is.EqualTo(Outcome.Failed));
                Assert.That(result.Summary.Title, Is.EqualTo(results[0].Summary.Title));
                Assert.That(result.Summary.Owner, Is.EqualTo(results[0].Summary.Owner));
                Assert.That(result.Summary.EndTime, Is.EqualTo(results[0].Summary.EndTime));

                Assert.That(result.Assemblies.Count(), Is.EqualTo(2));

                var assembly1 = result.Assemblies.First();
                Assert.That(assembly1.Title, Is.EqualTo(results[0].Assemblies.First().Title));
                Assert.That(assembly1.ParentKey, Is.EqualTo(results[0].Assemblies.First().ParentKey));

                var assembly2 = result.Assemblies.Last();
                Assert.That(assembly2.Title, Is.EqualTo(results[1].Assemblies.First().Title));
                Assert.That(assembly2.ParentKey, Is.EqualTo(results[1].Assemblies.First().ParentKey));

                Assert.That(result.Classes.Count(), Is.EqualTo(2));

                var class1 = result.Classes.First();
                Assert.That(class1.Title, Is.EqualTo(results[0].Classes.First().Title));
                Assert.That(class1.ParentKey, Is.EqualTo(results[0].Classes.First().ParentKey));

                var class2 = result.Classes.Last();
                Assert.That(class2.Title, Is.EqualTo(results[1].Classes.First().Title));
                Assert.That(class2.ParentKey, Is.EqualTo(results[1].Classes.First().ParentKey));

                Assert.That(result.TestCases.Count(), Is.EqualTo(2));

                var testCase1 = result.TestCases.First();
                Assert.That(testCase1.Title, Is.EqualTo(results[0].TestCases.First().Title));
                Assert.That(testCase1.ParentKey, Is.EqualTo(results[0].TestCases.First().ParentKey));

                var testCase2 = result.TestCases.Last();
                Assert.That(testCase2.Title, Is.EqualTo(results[1].TestCases.First().Title));
                Assert.That(testCase2.ParentKey, Is.EqualTo(results[1].TestCases.First().ParentKey));
            });
        });
    }
}
