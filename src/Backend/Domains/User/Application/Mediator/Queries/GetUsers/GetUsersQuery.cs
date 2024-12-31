using Backend.Domains.User.Domain.Entities;
using SaveApis.Core.Infrastructure.Mediator.Queries;

namespace Backend.Domains.User.Application.Mediator.Queries.GetUsers;

public record GetUsersQuery : IQuery<IEnumerable<UserEntity>>;
