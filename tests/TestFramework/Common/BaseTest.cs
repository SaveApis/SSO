using DotNet.Testcontainers.Containers;
using TestFramework.Common.Fixtures;

namespace TestFramework.Common;

[TestFixture]
public abstract class BaseTest(string testScope)
{
    protected static DockerFixture? Docker { get; private set; }

    [OneTimeSetUp]
    public virtual Task OneTimeSetUp()
    {
        Docker ??= new DockerFixture(testScope);

        if (Docker.MySql.State != TestcontainersStates.Running)
        {
            Docker.Start();

            // Ensuer containers are completed
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        return Task.CompletedTask;
    }

    [SetUp]
    public virtual Task SetUp()
    {
        return Task.CompletedTask;
    }

    [OneTimeTearDown]
    public virtual Task OneTimeTearDown()
    {
        return Task.CompletedTask;
    }

    [TearDown]
    public virtual Task TearDown()
    {
        return Task.CompletedTask;
    }
}