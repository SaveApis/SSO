using Backend.Domains.Common.Domain.VO;
using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.Project.Application.Hangfire.Events;
using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.Project.Domain.VO;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.Project.Application.Mediator.Commands.CreateProject;

public class CreateProjectCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<CreateProjectCommand, ProjectId>
{
    public async Task<Result<ProjectId>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var id = ProjectId.From(request.Dto.Id);
        var name = Name.From(request.Dto.Name);
        var description = Description.From(request.Dto.Description);

        var users = new List<UserEntity>();
        foreach (var userId in request.Dto.Users)
        {
            var user = await context.Users.FindAsync([UserId.From(userId)], cancellationToken).ConfigureAwait(false);
            if (user is null)
            {
                continue;
            }

            users.Add(user);
        }

        var entity = ProjectEntity.Create(id, name, description, users.ToArray());

        context.Projects.Add(entity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await PublishCreatedEvent(entity, cancellationToken).ConfigureAwait(false);

        return id;
    }

    private async Task PublishCreatedEvent(ProjectEntity entity, CancellationToken cancellationToken)
    {
        await mediator.Publish(new ProjectCreatedEvent(entity.Id.Value), cancellationToken).ConfigureAwait(false);
    }
}
