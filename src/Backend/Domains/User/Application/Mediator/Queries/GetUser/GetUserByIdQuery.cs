using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Queries;

namespace Backend.Domains.User.Application.Mediator.Queries.GetUser;

public record GetUserByIdQuery(UserId Id) : IQuery<UserEntity>;
