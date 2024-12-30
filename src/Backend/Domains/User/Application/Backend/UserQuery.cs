using Backend.Domains.User.Application.Mapper;
using Backend.Domains.User.Application.Mediator.Queries.GetUser;
using Backend.Domains.User.Application.Mediator.Queries.GetUsers;
using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.VO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;

namespace Backend.Domains.User.Application.Backend;

[QueryType]
public static class UserQuery
{
    [UseSorting]
    [UseFiltering]
    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager)])]
    public static async Task<IEnumerable<UserGetDto>> GetUsers([Service] IMediator mediator)
    {
        var mapper = new UserMapper();

        var queryResult = await mediator.Send(new GetUsersQuery()).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager)])]
    public static async Task<UserGetDto> GetUserById([Service] IMediator mediator, Guid id)
    {
        var mapper = new UserMapper();

        var queryResult = await mediator.Send(new GetUserByIdQuery(UserId.From(id))).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }
}
