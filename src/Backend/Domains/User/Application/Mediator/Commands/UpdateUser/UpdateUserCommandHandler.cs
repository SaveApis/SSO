using Backend.Domains.User.Application.Hangfire.Events;
using Backend.Domains.User.Application.Mediator.Errors;
using Backend.Domains.User.Domain.VO;
using Backend.Persistence.Sql;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Commands.UpdateUser;

public class UpdateUserCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<UpdateUserCommand, UserId>
{
    public async Task<Result<UserId>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var firstName = Name.From(dto.FirstName);
        var lastName = Name.From(dto.LastName);
        var email = Email.From(dto.Email);
        var phone = dto.Phone is null ? null : Phone.From(dto.Phone);

        await using var context = manager.Create<DataContext>();

        var user = await context.Users.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return new UserNotFoundError(request.Id);
        }

        user.WithFirstName(firstName).WithLastName(lastName).WithEmail(email).WithPhone(phone).WithUserName(dto.UserName);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await mediator.Publish(new UserChangedEvent(user.Id.Value), cancellationToken).ConfigureAwait(false);

        return user.Id;
    }
}
