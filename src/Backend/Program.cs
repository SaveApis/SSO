using System.Reflection;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Extensions;
using SaveApis.Web.Domains.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var isBackend = builder.Configuration.IsBackend();
var hangfireType = builder.Configuration.GetHangfireType();
builder.WithAssemblies(Assembly.GetExecutingAssembly()).WithSaveApis(hangfireType, isBackend);

var app = builder.Build();

await app.RunSaveApisAsync(isBackend).ConfigureAwait(false);
