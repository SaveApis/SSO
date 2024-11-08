using Autofac;
using Microsoft.Extensions.Configuration;
using SaveApis.Core.Application.DI;
using SaveApis.Core.Application.Extensions;
using TestFramework.Common.Fixtures;

namespace TestFramework.Integration.Fixtures;

public sealed class ContainerFixture(DockerFixture? docker)
{
    private static ContainerFixture? _instance;

    private DockerFixture Docker { get; } = docker ?? throw new ArgumentNullException(nameof(docker));
    private IContainer? Container { get; set; }

    public T Resolve<T>() where T : notnull
    {
        Container ??= BuildContainer();

        return Container.Resolve<T>();
    }


    private IContainer BuildContainer()
    {
        var configuration = Docker?.LoadConfiguration();

        var builder = new ContainerBuilder();
        builder.RegisterInstance(configuration).As<IConfiguration>().SingleInstance();

        builder.WithAssemblies(typeof(Program).Assembly);
        builder.RegisterModule(new SerilogModule(configuration));
        builder.RegisterModule(new EfCoreModule(configuration));
        builder.RegisterModule(new MediatorModule(configuration));
        builder.RegisterModule(new FluentValidatorModule(configuration));
        builder.RegisterModule(new EasyCachingModule(configuration));
        builder.RegisterModule(new HangfireModule(configuration));

        return builder.Build();
    }

    public static ContainerFixture Instance(DockerFixture? dockerFixture)
    {
        return _instance ??= new ContainerFixture(dockerFixture);
    }
}