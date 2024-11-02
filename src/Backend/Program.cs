using SaveApis.Core.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WithAssemblies().WithAutofac();

builder.Services.AddControllers();

var app = builder.Build();

await app.RunSaveApisAsync();