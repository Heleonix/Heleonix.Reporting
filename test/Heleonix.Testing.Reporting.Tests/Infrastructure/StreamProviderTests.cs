// <copyright file="StreamProviderTests.cs" company="Heleonix - Hennadii Lutsyshyn">
// Copyright (c) Heleonix - Hennadii Lutsyshyn. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the repository root for full license information.
// </copyright>

namespace Heleonix.Testing.Reporting.Tests.Infrastructure;

using Heleonix.Testing.Reporting.Infrastructure;

/// <summary>
/// Tests the <see cref="StreamProvider"/>.
/// </summary>
[ComponentTest(Type = typeof(StreamProvider))]
internal class StreamProviderTests
{
    /// <summary>
    /// Tests the <see cref="StreamProvider.OpenInputStream"/>.
    /// </summary>
    [MemberTest(Name = nameof(StreamProvider.OpenInputStream))]
    public void OpenInputStream()
    {
        StreamProvider provider = null;

        string input = null;

        Stream result = null;

        When("the method is called", () =>
        {
            Act(() =>
            {
                provider = new StreamProvider();

                result = provider.OpenInputStream(input);
            });

            And("the input file exists", () =>
            {
                Arrange(() =>
                {
                    input = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());

                    File.Create(input).Close();
                });

                Teardown(() =>
                {
                    result.Close();

                    File.Delete(input);
                });

                Should("open the stream for reading", () =>
                {
                    Assert.That(result.CanRead);
                });
            });
        });
    }

    /// <summary>
    /// Tests the <see cref="StreamProvider.OpenOutputStream"/>.
    /// </summary>
    [MemberTest(Name = nameof(StreamProvider.OpenOutputStream))]
    public void OpenOutputStream()
    {
        StreamProvider provider = null;

        var output = string.Empty;

        Stream result = null;

        When("the method is called", () =>
        {
            Act(() =>
            {
                provider = new StreamProvider();

                result = provider.OpenOutputStream(output);
            });

            And("the containing directory does not exist", () =>
            {
                Arrange(() =>
                {
                    output = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName(), Path.GetRandomFileName());
                });

                Teardown(() =>
                {
                    result.Close();

                    Directory.Delete(Path.GetDirectoryName(output), true);
                });

                Should("open the stream for writing", () =>
                {
                    Assert.That(result.CanWrite);
                });
            });

            And("the containing directory exists", () =>
            {
                Arrange(() =>
                {
                    output = Path.Combine(Environment.CurrentDirectory, Path.GetRandomFileName());
                });

                Teardown(() =>
                {
                    result.Close();

                    File.Delete(output);
                });

                Should("open the stream for writing", () =>
                {
                    Assert.That(result.CanWrite);
                });
            });
        });
    }
}
