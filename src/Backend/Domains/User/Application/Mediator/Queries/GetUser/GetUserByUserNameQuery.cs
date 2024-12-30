using Backend.Domains.User.Domain.Entities;
using SaveApis.Core.Infrastructure.Mediator.Queries;

namespace Backend.Domains.User.Application.Mediator.Queries.GetUser;

public record GetUserByUserNameQuery(string UserName) : IQuery<UserEntity>;
