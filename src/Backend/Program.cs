using System.Reflection;
using SaveApis.Web.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var applicationType = builder.ReadApplicationType();
builder.WithAssemblies(Assembly.GetExecutingAssembly()).WithSaveApis(applicationType);

var app = builder.Build();

await app.RunSaveApisAsync(applicationType).ConfigureAwait(false);
