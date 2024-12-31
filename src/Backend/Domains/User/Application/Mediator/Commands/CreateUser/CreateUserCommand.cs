using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.DTO;
using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.User.Application.Mediator.Commands.CreateUser;

public record CreateUserCommand(UserCreateDto Dto, SsoRole Role = SsoRole.User, bool IsInitialUser = false) : ICommand<UserId>;
