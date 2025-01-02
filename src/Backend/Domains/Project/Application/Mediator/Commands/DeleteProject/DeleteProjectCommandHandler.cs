using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.Project.Application.Hangfire.Events;
using Backend.Domains.Project.Application.Mediator.Errors;
using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.Project.Domain.VO;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.Project.Application.Mediator.Commands.DeleteProject;

public class DeleteProjectCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<DeleteProjectCommand, ProjectId>
{
    public async Task<Result<ProjectId>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var project = await context.Projects.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (project is null)
        {
            return new ProjectNotFoundError(request.Id);
        }

        context.Projects.Remove(project);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await PublishDeletedEvent(project, cancellationToken).ConfigureAwait(false);

        return project.Id;
    }

    private async Task PublishDeletedEvent(ProjectEntity project, CancellationToken cancellationToken)
    {
        await mediator.Publish(new ProjectDeletedEvent(project.Id.Value), cancellationToken).ConfigureAwait(false);
    }
}
