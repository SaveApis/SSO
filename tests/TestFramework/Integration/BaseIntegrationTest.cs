using MediatR;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Infrastructure.Persistence.MySql.Interfaces;
using TestFramework.Common;
using TestFramework.Integration.Fixtures;

namespace TestFramework.Integration;

public abstract class BaseIntegrationTest() : BaseTest("integration")
{
    protected static ContainerFixture Container => ContainerFixture.Instance(Docker);
    protected static IConfiguration Configuration => Container.Resolve<IConfiguration>();
    protected static IMediator Mediator => Container.Resolve<IMediator>();

    public override async Task SetUp()
    {
        await base.SetUp();

        Configuration["MYSQL_DATABASE"] = TestContext.CurrentContext.Test.MethodName;

        var factory = Container.Resolve<IDbContextFactory>();
        foreach (var context in factory.CreateAll()) await context.Database.EnsureCreatedAsync();
    }

    public override async Task TearDown()
    {
        await base.TearDown();

        var factory = Container.Resolve<IDbContextFactory>();
        foreach (var context in factory.CreateAll()) await context.Database.EnsureDeletedAsync();
    }
}