using Backend.Domains.Project.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.Project.Application.Mediator.Commands.DeleteProject;

public record DeleteProjectCommand(ProjectId Id) : ICommand<ProjectId>;
