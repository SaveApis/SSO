using Backend.Domains.User.Domain.VO;
using FluentResults;

namespace Backend.Domains.User.Application.Mediator.Errors;

public class UserNotFoundError : Error
{
    public UserNotFoundError(UserId id) : base($"User with id not found! ({id})")
    {
    }

    public UserNotFoundError(string userName) : base($"User with username not found! ({userName})")
    {
    }
}
