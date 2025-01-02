using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.Project.Application.Hangfire.Events;
using Backend.Domains.Project.Application.Mediator.Errors;
using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.Project.Domain.VO;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.Project.Application.Mediator.Commands.ActivateProject;

public class ActivateProjectCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<ActivateProjectCommand, ProjectId>
{
    public async Task<Result<ProjectId>> Handle(ActivateProjectCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var project = await context.Projects.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (project is null)
        {
            return new ProjectNotFoundError(request.Id);
        }

        project.Activate();

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await PublishChangedEvent(project, cancellationToken).ConfigureAwait(false);

        return project.Id;
    }

    private async Task PublishChangedEvent(ProjectEntity project, CancellationToken cancellationToken = default)
    {
        await mediator.Publish(new ProjectChangedEvent(project.Id.Value), cancellationToken).ConfigureAwait(false);
    }
}
