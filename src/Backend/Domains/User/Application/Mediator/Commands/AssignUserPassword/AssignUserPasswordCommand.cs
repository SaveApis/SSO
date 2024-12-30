using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.User.Application.Mediator.Commands.AssignUserPassword;

public record AssignUserPasswordCommand(UserId Id, string Password) : ICommand<UserId>;
