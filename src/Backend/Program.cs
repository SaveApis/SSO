using SaveApis.Core.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WithAssemblies().WithAutofac((containerBuilder, configuration) => containerBuilder.WithSwagger(configuration));

builder.Services.AddControllers();

var app = builder.Build();

await app.RunSaveApisAsync();