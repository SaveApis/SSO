using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.User.Application.Mediator.Errors;
using Backend.Domains.User.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Mediator.Queries;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Queries.GetUser;

public class GetUserQueryHandler(IDbManager manager) : IQueryHandler<GetUserByIdQuery, UserEntity>, IQueryHandler<GetUserByUserNameQuery, UserEntity>
{
    public async Task<Result<UserEntity>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var existingUser = await context.Users.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);

        return existingUser ?? (Result<UserEntity>)new UserNotFoundError(request.Id);
    }

    public async Task<Result<UserEntity>> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var existingUser = await context.Users.SingleOrDefaultAsync(x => string.Equals(x.UserName, request.UserName, StringComparison.InvariantCultureIgnoreCase), cancellationToken).ConfigureAwait(false);

        return existingUser ?? (Result<UserEntity>)new UserNotFoundError(request.UserName);
    }
}
