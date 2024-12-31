using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.User.Application.Mediator.Commands.UpdateUser;

public record UpdateUserCommand(UserId Id, UserUpdateDto Dto) : ICommand<UserId>;
