using System.Security.Claims;
using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;

namespace Backend.Domains.Common.Infrastructure.Extensions;

public static class HttpContextAccessorExtensions
{
    public static UserId GetUserId(this IHttpContextAccessor contextAccessor)
    {
        var value = contextAccessor.HttpContext?.User.FindFirstValue(nameof(UserEntity.Id));
        if (value is null)
        {
            throw new InvalidDataException("No ID claim is set");
        }

        return UserId.From(Guid.Parse(value));
    }

    public static bool IsInRole(this IHttpContextAccessor contextAccessor, params SsoRole[] roles)
    {
        return roles.Any(role => contextAccessor.HttpContext?.User.IsInRole(role.ToString()) == true);
    }
}
