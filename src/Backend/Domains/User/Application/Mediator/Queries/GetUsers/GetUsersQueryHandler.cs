using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.User.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Mediator.Queries;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Queries.GetUsers;

public class GetUsersQueryHandler(IDbManager manager) : IQueryHandler<GetUsersQuery, IEnumerable<UserEntity>>
{
    public async Task<Result<IEnumerable<UserEntity>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        return await context.Users.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}
