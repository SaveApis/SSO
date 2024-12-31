using FluentResults;

namespace Backend.Domains.User.Application.Mediator.Commands.DeleteUser.Errors;

public class UserDeletionError() : Error("Initial user cannot be deleted!");
