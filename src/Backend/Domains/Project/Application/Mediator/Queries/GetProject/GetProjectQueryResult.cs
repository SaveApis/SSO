using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.Project.Application.Mediator.Errors;
using Backend.Domains.Project.Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Mediator.Queries;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.Project.Application.Mediator.Queries.GetProject;

public class GetProjectQueryResult(IDbManager manager) : IQueryHandler<GetProjectByIdQuery, ProjectEntity>
{
    public async Task<Result<ProjectEntity>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var existingProject = await context.Projects.Include(e => e.Users).FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken).ConfigureAwait(false);

        return existingProject ?? (Result<ProjectEntity>)new ProjectNotFoundError(request.Id);
    }
}
