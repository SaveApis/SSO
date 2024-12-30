using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.User.Application.Mediator.Commands.DeactivateUser;

public record DeactivateUserCommand(UserId Id) : ICommand<UserId>;
