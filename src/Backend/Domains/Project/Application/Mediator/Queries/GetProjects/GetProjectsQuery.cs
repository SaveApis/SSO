using System.Linq.Expressions;
using Backend.Domains.Project.Domain.Entities;
using SaveApis.Core.Infrastructure.Mediator.Queries;

namespace Backend.Domains.Project.Application.Mediator.Queries.GetProjects;

public record GetProjectsQuery(Expression<Func<ProjectEntity, bool>>? Filter = null) : IQuery<IEnumerable<ProjectEntity>>;
