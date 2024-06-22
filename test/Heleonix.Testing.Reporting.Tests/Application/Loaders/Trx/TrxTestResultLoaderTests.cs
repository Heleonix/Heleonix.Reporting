// <copyright file="TrxTestResultLoaderTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Application.Loaders.Trx;

using Heleonix.Testing.Reporting.Application;
using Heleonix.Testing.Reporting.Application.Loaders.Trx;
using Heleonix.Testing.Reporting.Domain;
using Heleonix.Testing.Reporting.Tests.Samples;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Text;

/// <summary>
/// Tests the <see cref="TrxTestResultLoader"/>.
/// </summary>
[ComponentTest(Type = typeof(TrxTestResultLoader))]
internal class TrxTestResultLoaderTests
{
    /// <summary>
    /// Tests the <see cref="TrxTestResultLoader.Load"/>.
    /// </summary>
    [MemberTest(Name = nameof(TrxTestResultLoader.Load))]
    public void Load()
    {
        IStreamProvider streamProvider = null;
        ILogger<TrxTestResultLoader> logger = null;

        TrxTestResultLoader trxTestResultLoader = null;

        string filePath = null;

        TestResult result = null;

        When("the method is called", () =>
        {
            Arrange(() =>
            {
                filePath = PathHelper.SampleTrx;
                logger = Mock.Of<ILogger<TrxTestResultLoader>>();
            });

            Act(() =>
            {
                trxTestResultLoader = new TrxTestResultLoader(streamProvider, logger);

                result = trxTestResultLoader.Load(filePath);
            });

            And("the valid TRX test result file is provided", () =>
            {
                FileStream fileStream = null;

                Arrange(() =>
                {
                    fileStream = File.OpenRead(filePath);

                    streamProvider = Mock.Of<IStreamProvider>(sp => sp.OpenInputStream(filePath) == fileStream);
                });

                Teardown(() => fileStream.Dispose());

                Should("successfully load the test result", () =>
                {
                    Assert.That(result.Summary.Title, Is.Not.Empty);
                    Assert.That(result.Summary.Owner, Is.Not.Empty);
                    Assert.That(result.Summary.Outcome, Is.EqualTo(Outcome.Failed));
                    Assert.That(result.Summary.EndTime.Year, Is.EqualTo(2024));

                    var assembly = result.Assemblies.Single();
                    Assert.That(assembly.Title, Is.EqualTo("Heleonix.Testing.Reporting.Tests"));

                    var successfulTests = result.Classes.Single(t => t.Title == nameof(SuccessfulTests));
                    var failedTests = result.Classes.Single(t => t.Title == nameof(FailedTests));
                    var warningTests = result.Classes.Single(t => t.Title == nameof(WarningTests));
                    Assert.That(result.Classes.Count(), Is.EqualTo(3));
                    Assert.That(successfulTests.ParentKey, Is.EqualTo(assembly.Key));
                    Assert.That(failedTests.ParentKey, Is.EqualTo(assembly.Key));
                    Assert.That(warningTests.ParentKey, Is.EqualTo(assembly.Key));

                    var successfulTest1 = result.TestCases.Single(tc => tc.Title == nameof(SuccessfulTests.SuccessfulTest1));
                    var successfulTest2 = result.TestCases.Single(tc => tc.Title == nameof(SuccessfulTests.SuccessfulTest2));
                    var failedTest1 = result.TestCases.Single(tc => tc.Title == nameof(FailedTests.FailedTest1));
                    var failedTest2 = result.TestCases.Single(tc => tc.Title == nameof(FailedTests.FailedTest2));
                    var warningTest1 = result.TestCases.Single(tc => tc.Title == nameof(WarningTests.WarningTest1));
                    Assert.That(result.TestCases.Count(), Is.EqualTo(5));

                    Assert.That(successfulTest1.ParentKey, Is.EqualTo(successfulTests.Key));
                    Assert.That(successfulTest1.Start, Is.LessThan(result.Summary.EndTime));
                    Assert.That(successfulTest1.End, Is.GreaterThan(successfulTest1.Start));
                    Assert.That(successfulTest1.Duration, Is.Not.Zero);
                    Assert.That(successfulTest1.Outcome, Is.EqualTo(Outcome.Passed));
                    Assert.That(successfulTest1.Output, Is.Not.Empty);
                    Assert.That(successfulTest1.Errors, Is.Empty);

                    Assert.That(successfulTest2.ParentKey, Is.EqualTo(successfulTests.Key));
                    Assert.That(successfulTest2.Start, Is.LessThan(result.Summary.EndTime));
                    Assert.That(successfulTest2.End, Is.GreaterThan(successfulTest2.Start));
                    Assert.That(successfulTest2.Duration, Is.Zero);
                    Assert.That(successfulTest2.Outcome, Is.EqualTo(Outcome.Passed));
                    Assert.That(successfulTest2.Output, Is.Empty);
                    Assert.That(successfulTest2.Errors, Is.Empty);

                    Assert.That(failedTest1.ParentKey, Is.EqualTo(failedTests.Key));
                    Assert.That(failedTest1.Start, Is.LessThan(result.Summary.EndTime));
                    Assert.That(failedTest1.End, Is.GreaterThan(failedTest1.Start));
                    Assert.That(failedTest1.Duration, Is.Not.Zero);
                    Assert.That(failedTest1.Outcome, Is.EqualTo(Outcome.Failed));
                    Assert.That(failedTest1.Output, Is.Not.Empty);
                    Assert.That(failedTest1.Errors, Is.Not.Empty);

                    Assert.That(failedTest2.ParentKey, Is.EqualTo(failedTests.Key));
                    Assert.That(failedTest2.Start, Is.LessThan(result.Summary.EndTime));
                    Assert.That(failedTest2.End, Is.GreaterThan(failedTest2.Start));
                    Assert.That(failedTest2.Duration, Is.Not.Zero);
                    Assert.That(failedTest2.Outcome, Is.EqualTo(Outcome.Failed));
                    Assert.That(failedTest2.Output, Is.Empty);
                    Assert.That(failedTest2.Errors, Is.Not.Empty);

                    Assert.That(warningTest1.ParentKey, Is.EqualTo(warningTests.Key));
                    Assert.That(warningTest1.Start, Is.LessThan(result.Summary.EndTime));
                    Assert.That(warningTest1.End, Is.GreaterThan(warningTest1.Start));
                    Assert.That(warningTest1.Duration, Is.Not.Zero);
                    Assert.That(warningTest1.Outcome, Is.EqualTo(Outcome.Other));
                    Assert.That(warningTest1.Output, Is.Not.Empty);
                    Assert.That(warningTest1.Errors, Is.Not.Empty);
                });
            });

            And("the invalid TRX test result file is provided", () =>
            {
                MemoryStream stream = null;

                Arrange(() =>
                {
                    stream = new MemoryStream(Encoding.ASCII.GetBytes("INVALID TRX"));

                    streamProvider = Mock.Of<IStreamProvider>(sp => sp.OpenInputStream(filePath) == stream);
                });

                Teardown(() =>
                {
                    stream.Dispose();
                });

                Should("return null", () =>
                {
                    Assert.That(result, Is.Null);
                });
            });
        });
    }
}
