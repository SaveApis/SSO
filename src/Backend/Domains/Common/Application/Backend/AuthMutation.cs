using Backend.Domains.Project.Application.Mediator.Queries.GetProject;
using Backend.Domains.Project.Domain.DTO;
using Backend.Domains.Project.Domain.VO;
using Backend.Domains.User.Application.Mediator.Queries.GetUser;
using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.Entities;
using HotChocolate;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Core.Infrastructure.Jwt.Builder;

namespace Backend.Domains.Common.Application.Backend;

[MutationType]
public static class AuthMutation
{
    public static async Task<string> Login([Service] IJwtBuilder builder, [Service] IMediator mediator, UserLoginDto dto)
    {
        var result = await mediator.Send(new GetUserByUserNameQuery(dto.UserName)).ConfigureAwait(false);
        result.ThrowIfFailed();

        if (!result.Value.ValidatePassword(dto.Password))
        {
            throw new UnauthorizedAccessException("Invalid password!");
        }

        return await builder.WithClaim(nameof(UserEntity.Id), result.Value.Id.Value.ToString()).WithRole(result.Value.Role.ToString()).BuildAsync().ConfigureAwait(false);
    }

    public static async Task<bool> ProjectLogin([Service] IMediator mediator, ProjectLoginDto dto)
    {
        var userQueryResult = await mediator.Send(new GetUserByUserNameQuery(dto.UserName)).ConfigureAwait(false);
        userQueryResult.ThrowIfFailed();

        if (!userQueryResult.Value.ValidatePassword(dto.Password))
        {
            throw new UnauthorizedAccessException("Invalid password!");
        }

        var projectQueryResult = await mediator.Send(new GetProjectByIdQuery(ProjectId.From(dto.Id))).ConfigureAwait(false);
        projectQueryResult.ThrowIfFailed();

        if (userQueryResult.Value.Role is SsoRole.Administrator or SsoRole.Developer)
        {
            return true;
        }

        if (projectQueryResult.Value.Users.All(x => x.Id != userQueryResult.Value.Id))
        {
            throw new UnauthorizedAccessException("User is not assigned to this project!");
        }

        return true;
    }
}
