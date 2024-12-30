using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.User.Application.Mediator.Commands.DeleteUser;

public record DeleteUserCommand(UserId Id) : ICommand<UserId>;
