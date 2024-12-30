using Backend.Domains.Mail.Application.Hangfire.Events;
using Backend.Domains.Mail.Domain;
using Backend.Domains.User.Application.Hangfire.Events;
using Backend.Domains.User.Application.Mediator.Errors;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using Backend.Persistence.Sql;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Commands.AssignUserPassword;

public class AssignUserPasswordCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<AssignUserPasswordCommand, UserId>
{
    public async Task<Result<UserId>> Handle(AssignUserPasswordCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var user = await context.Users.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return new UserNotFoundError(request.Id);
        }

        user.WithPassword(request.Password);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await SendChangedEvent(user, cancellationToken).ConfigureAwait(false);
        await SendMailEvent(user, request.Password, cancellationToken).ConfigureAwait(false);

        return user.Id;
    }

    private async Task SendMailEvent(UserEntity user, string password, CancellationToken cancellationToken)
    {
        var properties = new Dictionary<string, object>
        {
            { MailPropertyNames.FullName, user.FullName },
            { MailPropertyNames.UserName, user.UserName },
            { MailPropertyNames.Password, password },
        };
        var @event = new SendEmailEvent(user.Email.Value, MailTemplateNames.NewPassword, properties);

        await mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
    }

    private async Task SendChangedEvent(UserEntity user, CancellationToken cancellationToken)
    {
        await mediator.Publish(new UserChangedEvent(user.Id.Value), cancellationToken).ConfigureAwait(false);
    }
}
