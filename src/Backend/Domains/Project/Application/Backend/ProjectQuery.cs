using Backend.Domains.Common.Application.Mapper;
using Backend.Domains.Common.Infrastructure.Extensions;
using Backend.Domains.Project.Application.Mediator.Queries.GetProject;
using Backend.Domains.Project.Application.Mediator.Queries.GetProjects;
using Backend.Domains.Project.Domain.DTO;
using Backend.Domains.Project.Domain.VO;
using Backend.Domains.User.Domain;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using SaveApis.Core.Infrastructure.Extensions;

namespace Backend.Domains.Project.Application.Backend;

[QueryType]
public static class ProjectQuery
{
    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager), nameof(SsoRole.User)])]
    public static async Task<IEnumerable<ProjectGetDto>> GetProjects([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, CancellationToken cancellationToken = default)
    {
        var mapper = new SsoMapper();

        var userId = contextAccessor.GetUserId();
        var queryResult = contextAccessor.IsInRole(SsoRole.User, SsoRole.Manager)
            ? await mediator.Send(new GetProjectsQuery(entity => entity.Users.Any(it => it.Id == userId)), cancellationToken).ConfigureAwait(false)
            : await mediator.Send(new GetProjectsQuery(), cancellationToken).ConfigureAwait(false);

        return mapper.ToDto(queryResult.Value);
    }

    [Authorize(Roles = [nameof(SsoRole.Developer), nameof(SsoRole.Administrator), nameof(SsoRole.Manager), nameof(SsoRole.User)])]
    public static async Task<ProjectGetDto> GetProjectById([Service] IMediator mediator, [Service] IHttpContextAccessor contextAccessor, string id, CancellationToken cancellationToken = default)
    {
        var mapper = new SsoMapper();
        var userId = contextAccessor.GetUserId();

        var queryResult = await mediator.Send(new GetProjectByIdQuery(ProjectId.From(id)), cancellationToken).ConfigureAwait(false);
        queryResult.ThrowIfFailed();

        if (contextAccessor.IsInRole(SsoRole.User, SsoRole.Manager) && queryResult.Value.Users.All(it => it.Id != userId))
        {
            throw new UnauthorizedAccessException("You are not allowed to access this project!");
        }

        return mapper.ToDto(queryResult.Value);
    }
}
