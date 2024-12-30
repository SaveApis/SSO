using Backend.Domains.User.Application.Mediator.Queries.GetUser;
using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.Entities;
using HotChocolate;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Core.Infrastructure.Jwt.Builder;

namespace Backend.Application.Backend;

[MutationType]
public static class AuthMutation
{
    public static async Task<string> Login([Service] IJwtBuilder builder, [Service] IMediator mediator, UserLoginDto dto)
    {
        var result = await mediator.Send(new GetUserByUserNameQuery(dto.UserName)).ConfigureAwait(false);
        result.ThrowIfFailed();

        if (!result.Value.ValidatePassword(dto.Password))
        {
            throw new UnauthorizedAccessException("Invalid password");
        }

        return await builder.WithClaim(nameof(UserEntity.Id), result.Value.Id.Value.ToString()).WithRole(result.Value.Role.ToString()).BuildAsync().ConfigureAwait(false);
    }
}
