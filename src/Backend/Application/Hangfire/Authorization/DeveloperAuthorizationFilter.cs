using System.Net.Http.Headers;
using System.Text;
using Backend.Domains.User.Application.Mediator.Queries.GetUser;
using Hangfire.Dashboard;
using MediatR;

namespace Backend.Application.Hangfire.Authorization;

public class DeveloperAuthorizationFilter(IMediator mediator) : IDashboardAsyncAuthorizationFilter
{
    public async Task<bool> AuthorizeAsync(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            return true;
        }

        if (!httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            return BasicAuth(httpContext);
        }

        var authHeader = AuthenticationHeaderValue.Parse(httpContext.Request.Headers.Authorization!);
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':');

        var userName = credentials[0];
        var password = credentials[1];

        var queryResult = await mediator.Send(new GetUserByUserNameQuery(userName)).ConfigureAwait(false);
        if (queryResult.IsFailed)
        {
            return BasicAuth(httpContext);
        }

        return queryResult.Value.ValidatePassword(password) || BasicAuth(httpContext);
    }

    private static bool BasicAuth(HttpContext context)
    {
        context.Response.Headers.WWWAuthenticate = "Basic realm=\"Hangfire Dashboard\"";
        context.Response.StatusCode = 401;

        return false;
    }
}
