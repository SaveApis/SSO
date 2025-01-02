using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.Project.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Queries;

namespace Backend.Domains.Project.Application.Mediator.Queries.GetProject;

public record GetProjectByIdQuery(ProjectId Id) : IQuery<ProjectEntity>;
