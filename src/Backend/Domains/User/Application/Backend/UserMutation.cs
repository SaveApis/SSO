using Backend.Domains.User.Application.Mapper;
using Backend.Domains.User.Application.Mediator.Commands.ActivateUser;
using Backend.Domains.User.Application.Mediator.Commands.CreateUser;
using Backend.Domains.User.Application.Mediator.Commands.DeactivateUser;
using Backend.Domains.User.Application.Mediator.Commands.DeleteUser;
using Backend.Domains.User.Application.Mediator.Commands.UpdateUser;
using Backend.Domains.User.Application.Mediator.Queries.GetUser;
using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;

namespace Backend.Domains.User.Application.Backend;

[MutationType]
public static class UserMutation
{
    [Authorize(Roles = [nameof(SsoRole.Developer)])]
    public static async Task<UserGetDto> CreateDeveloperUser([Service] IMediator mediator, UserCreateDto dto)
    {
        var mapper = new UserMapper();

        var commandResult = await mediator.Send(new CreateUserCommand(dto, SsoRole.Developer)).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetUserByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer)])]
    public static async Task<UserGetDto> CreateAdministratorUser([Service] IMediator mediator, UserCreateDto dto)
    {
        var mapper = new UserMapper();

        var commandResult = await mediator.Send(new CreateUserCommand(dto, SsoRole.Administrator)).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetUserByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<UserGetDto> CreateManagerUser([Service] IMediator mediator, UserCreateDto dto)
    {
        var mapper = new UserMapper();

        var commandResult = await mediator.Send(new CreateUserCommand(dto, SsoRole.Manager)).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetUserByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<UserGetDto> CreateUser([Service] IMediator mediator, UserCreateDto dto)
    {
        var mapper = new UserMapper();

        var commandResult = await mediator.Send(new CreateUserCommand(dto)).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetUserByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager), nameof(SsoRole.User)])]
    public static async Task<UserGetDto> UpdateUser([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, Guid id,
        UserUpdateDto dto)
    {
        var isSelfUser = contextAccessor.HttpContext?.User.Claims.Any(c => c.Type == nameof(UserEntity.Id) && c.Value == id.ToString());
        if (!isSelfUser.HasValue || !isSelfUser.Value)
        {
            var isDeveloper = contextAccessor.HttpContext?.User.IsInRole(nameof(SsoRole.Developer));
            var isAdministrator = contextAccessor.HttpContext?.User.IsInRole(nameof(SsoRole.Administrator));

            var isRole = (!isDeveloper.HasValue || !isDeveloper.Value) && (!isAdministrator.HasValue || !isAdministrator.Value);
            if (isRole)
            {
                throw new UnauthorizedAccessException();
            }
        }

        var mapper = new UserMapper();

        var commandResult = await mediator.Send(new UpdateUserCommand(UserId.From(id), dto)).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetUserByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<Guid> DeleteUser([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, Guid id)
    {
        var isSelfUser = contextAccessor.HttpContext?.User.Claims.Any(c => c.Type == nameof(UserEntity.Id) && c.Value == id.ToString());
        if (!isSelfUser.HasValue || isSelfUser.Value)
        {
            throw new InvalidOperationException("You cannot delete yourself.");
        }

        var commandResult = await mediator.Send(new DeleteUserCommand(UserId.From(id))).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        return commandResult.Value.Value;
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<UserGetDto> ActivateUser([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, Guid id)
    {
        var isOwnUser = contextAccessor.HttpContext?.User.Claims.Any(c => c.Type == nameof(UserEntity.Id) && c.Value == id.ToString());
        if (!isOwnUser.HasValue || isOwnUser.Value)
        {
            throw new InvalidOperationException("You cannot activate yourself.");
        }

        var mapper = new UserMapper();

        var commandResult = await mediator.Send(new ActivateUserCommand(UserId.From(id))).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetUserByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<UserGetDto> DeactivateUser([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor,
        Guid id)
    {
        var isOwnUser = contextAccessor.HttpContext?.User.Claims.Any(c => c.Type == nameof(UserEntity.Id) && c.Value == id.ToString());
        if (!isOwnUser.HasValue || isOwnUser.Value)
        {
            throw new InvalidOperationException("You cannot deactivate yourself.");
        }

        var mapper = new UserMapper();

        var commandResult = await mediator.Send(new DeactivateUserCommand(UserId.From(id))).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetUserByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }
}
