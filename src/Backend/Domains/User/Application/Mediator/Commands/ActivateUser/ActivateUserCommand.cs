using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.User.Application.Mediator.Commands.ActivateUser;

public record ActivateUserCommand(UserId Id) : ICommand<UserId>;
