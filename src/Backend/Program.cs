using System.Reflection;
using Backend.Domains.Mail.Application.DI;
using SaveApis.Core.Application.Jwt;
using SaveApis.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .WithAssemblies(Assembly.GetExecutingAssembly())
    .AddSaveApis(executorBuilder => executorBuilder.AddTypes().DisableIntrospection(false).ModifyCostOptions(options =>
        {
            options.MaxFieldCost = 1_000_000;
            options.MaxTypeCost = 1_000_000;
            options.EnforceCostLimits = true;
            options.ApplyCostDefaults = true;
            options.DefaultResolverCost = 10.0;
        }),
        AuthenticationMode.Jwt,
        (containerBuilder, configuration) => containerBuilder.WithModule<MailModule>(configuration));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

await app.RunSaveApisAsync(args).ConfigureAwait(false);
