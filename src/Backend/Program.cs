using System.Reflection;
using SaveApis.Core.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WithAssemblies(Assembly.GetExecutingAssembly())
    .WithAutofac();

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

await app.RunSaveApisAsync();