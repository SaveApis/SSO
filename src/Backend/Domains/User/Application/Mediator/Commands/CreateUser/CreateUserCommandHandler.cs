using Backend.Domains.Common.Domain.VO;
using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.Mail.Application.Hangfire.Events;
using Backend.Domains.Mail.Domain;
using Backend.Domains.User.Application.Hangfire.Events;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Commands.CreateUser;

public class CreateUserCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<CreateUserCommand, UserId>
{
    public async Task<Result<UserId>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var firstName = Name.From(dto.FirstName);
        var lastName = Name.From(dto.LastName);
        var email = Email.From(dto.Email);
        var phone = dto.Phone is null ? null : Phone.From(dto.Phone);

        var entity = UserEntity.Create(firstName, lastName, email, phone, request.Role, dto.UserName, true, request.IsInitialUser);

        await using var context = manager.Create<DataContext>();
        context.Users.Add(entity);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await PublishCreateEvent(entity, cancellationToken).ConfigureAwait(false);
        await Publish(entity, cancellationToken).ConfigureAwait(false);

        return entity.Id;
    }

    private async Task Publish(UserEntity entity, CancellationToken cancellationToken)
    {
        var properties = new Dictionary<string, object>
        {
            { MailPropertyNames.FullName, entity.FullName },
        };
        await mediator.Publish(new SendEmailEvent(entity.Email.Value, MailTemplateNames.Welcome, properties), cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task PublishCreateEvent(UserEntity entity, CancellationToken cancellationToken = default)
    {
        await mediator.Publish(new UserCreatedEvent(entity.Id.Value), cancellationToken).ConfigureAwait(false);
    }
}
