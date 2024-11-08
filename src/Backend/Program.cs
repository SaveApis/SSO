using System.Reflection;
using SaveApis.Core.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WithAutofac((containerBuilder, configuration) =>
    containerBuilder.WithAssemblies(Assembly.GetExecutingAssembly()).WithSwagger(configuration));

builder.Services.AddControllers();

var app = builder.Build();

await app.RunSaveApisAsync();