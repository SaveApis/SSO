using Backend.Domains.Project.Domain.VO;
using FluentResults;

namespace Backend.Domains.Project.Application.Mediator.Errors;

public class ProjectNotFoundError(ProjectId id) : Error($"Project with id not found! ({id})");
