using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.Project.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Mediator.Queries;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.Project.Application.Mediator.Queries.GetProjects;

public class GetProjectsQueryHandler(IDbManager manager) : IQueryHandler<GetProjectsQuery, IEnumerable<ProjectEntity>>
{
    public async Task<Result<IEnumerable<ProjectEntity>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var query = context.Projects.Include(e => e.Users);
        if (request.Filter is not null)
        {
            return await query.Where(request.Filter).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}
