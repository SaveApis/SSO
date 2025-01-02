using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.Mail.Application.Hangfire.Events;
using Backend.Domains.Mail.Domain;
using Backend.Domains.Project.Application.Hangfire.Events;
using Backend.Domains.Project.Application.Mediator.Errors;
using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.Project.Domain.VO;
using Backend.Domains.User.Domain.Entities;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.Project.Application.Mediator.Commands.AddUsersToProject;

public class AddUsersToProjectCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<AddUsersToProjectCommand, ProjectId>
{
    public async Task<Result<ProjectId>> Handle(AddUsersToProjectCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var project = await context.Projects
            .Include(p => p.Users)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            .ConfigureAwait(false);
        if (project is null)
        {
            return new ProjectNotFoundError(request.Id);
        }

        var users = context.Users
            .AsEnumerable()
            .Where(u => request.Users.Contains(u.Id))
            .ToList();

        project.AddUsers(users.ToArray());
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await PublishChangedEvent(project, cancellationToken).ConfigureAwait(false);

        foreach (var user in users)
        {
            await PublishMailEvent(project, user, cancellationToken).ConfigureAwait(false);
        }

        return project.Id;
    }

    private async Task PublishChangedEvent(ProjectEntity project, CancellationToken cancellationToken = default)
    {
        await mediator.Publish(new ProjectChangedEvent(project.Id.Value), cancellationToken).ConfigureAwait(false);
    }

    private async Task PublishMailEvent(ProjectEntity project, UserEntity user, CancellationToken cancellationToken)
    {
        var properties = new Dictionary<string, object>
        {
            { MailPropertyNames.FullName, user.FullName },
            { MailPropertyNames.ProjectName, project.Name.Value },
        };
        await mediator.Publish(new SendEmailEvent(user.Email.Value, MailTemplateNames.ProjectAssigned, properties), cancellationToken).ConfigureAwait(false);
    }
}
