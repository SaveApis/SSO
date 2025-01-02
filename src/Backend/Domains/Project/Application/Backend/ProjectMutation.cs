using Backend.Domains.Common.Application.Mapper;
using Backend.Domains.Common.Infrastructure.Extensions;
using Backend.Domains.Project.Application.Mediator.Commands.ActivateProject;
using Backend.Domains.Project.Application.Mediator.Commands.AddUsersToProject;
using Backend.Domains.Project.Application.Mediator.Commands.CreateProject;
using Backend.Domains.Project.Application.Mediator.Commands.DeactivateProject;
using Backend.Domains.Project.Application.Mediator.Commands.DeleteProject;
using Backend.Domains.Project.Application.Mediator.Commands.RemoveUsersFromProject;
using Backend.Domains.Project.Application.Mediator.Commands.UpdateProject;
using Backend.Domains.Project.Application.Mediator.Queries.GetProject;
using Backend.Domains.Project.Domain.DTO;
using Backend.Domains.Project.Domain.VO;
using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.VO;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;

namespace Backend.Domains.Project.Application.Backend;

[MutationType]
public static class ProjectMutation
{
    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<ProjectGetDto> CreateProject([Service] IMediator mediator, ProjectCreateDto dto, CancellationToken cancellationToken = default)
    {
        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new CreateProjectCommand(dto), cancellationToken).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value), cancellationToken).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<ProjectGetDto> UpdateProject([Service] IMediator mediator, string id, ProjectUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new UpdateProjectCommand(ProjectId.From(id), dto), cancellationToken).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value), cancellationToken).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<string> DeleteProject([Service] IMediator mediator, string id, CancellationToken cancellationToken = default)
    {
        var commandResult = await mediator.Send(new DeleteProjectCommand(ProjectId.From(id)), cancellationToken).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        return commandResult.Value.Value;
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<ProjectGetDto> ActivateProject([Service] IMediator mediator, string id, CancellationToken cancellationToken = default)
    {
        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new ActivateProjectCommand(ProjectId.From(id)), cancellationToken).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value), cancellationToken).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator)])]
    public static async Task<ProjectGetDto> DeactivateProject([Service] IMediator mediator, string id)
    {
        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new DeactivateProjectCommand(ProjectId.From(id))).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager)])]
    public static async Task<ProjectGetDto> AddUserToProject([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, string projectId, Guid userId)
    {
        var project = ProjectId.From(projectId);

        if (contextAccessor.IsInRole(SsoRole.Manager))
        {
            var managerId = contextAccessor.GetUserId();

            var projectQueryResult = await mediator.Send(new GetProjectByIdQuery(project)).ConfigureAwait(false);
            projectQueryResult.ThrowIfFailed();

            if (projectQueryResult.Value.Users.All(it => it.Id != managerId))
            {
                throw new UnauthorizedAccessException("You are not assigned to this Project!");
            }
        }

        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new AddUsersToProjectCommand(project, UserId.From(userId))).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager)])]
    public static async Task<ProjectGetDto> AddUsersToProject([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, string projectId, List<Guid> userIds)
    {
        var project = ProjectId.From(projectId);

        if (contextAccessor.IsInRole(SsoRole.Manager))
        {
            var managerId = contextAccessor.GetUserId();

            var projectQueryResult = await mediator.Send(new GetProjectByIdQuery(project)).ConfigureAwait(false);
            projectQueryResult.ThrowIfFailed();

            if (projectQueryResult.Value.Users.All(it => it.Id != managerId))
            {
                throw new UnauthorizedAccessException("You are not assigned to this Project!");
            }
        }

        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new AddUsersToProjectCommand(project, userIds.Select(UserId.From).ToArray())).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager)])]
    public static async Task<ProjectGetDto> RemoveUserFromProject([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, string projectId, Guid userId)
    {
        var project = ProjectId.From(projectId);

        if (contextAccessor.IsInRole(SsoRole.Manager))
        {
            var managerId = contextAccessor.GetUserId();

            var projectQueryResult = await mediator.Send(new GetProjectByIdQuery(project)).ConfigureAwait(false);
            projectQueryResult.ThrowIfFailed();

            if (projectQueryResult.Value.Users.All(it => it.Id != managerId))
            {
                throw new UnauthorizedAccessException("You are not assigned to this Project!");
            }
        }

        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new RemoveUsersFromProjectCommand(ProjectId.From(projectId), UserId.From(userId))).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager)])]
    public static async Task<ProjectGetDto> RemoveUsersFromProject([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, string projectId, List<Guid> userIds)
    {
        var project = ProjectId.From(projectId);

        if (contextAccessor.IsInRole(SsoRole.Manager))
        {
            var managerId = contextAccessor.GetUserId();

            var projectQueryResult = await mediator.Send(new GetProjectByIdQuery(project)).ConfigureAwait(false);
            projectQueryResult.ThrowIfFailed();

            if (projectQueryResult.Value.Users.All(it => it.Id != managerId))
            {
                throw new UnauthorizedAccessException("You are not assigned to this Project!");
            }
        }

        var mapper = new SsoMapper();

        var commandResult = await mediator.Send(new RemoveUsersFromProjectCommand(ProjectId.From(projectId), userIds.Select(UserId.From).ToArray())).ConfigureAwait(false);
        commandResult.ThrowIfFailed();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(commandResult.Value)).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        return mapper.ToDto(queryResult.Value);
    }
}
