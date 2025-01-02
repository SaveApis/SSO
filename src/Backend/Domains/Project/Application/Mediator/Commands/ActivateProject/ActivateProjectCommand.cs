using Backend.Domains.Project.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.Project.Application.Mediator.Commands.ActivateProject;

public record ActivateProjectCommand(ProjectId Id) : ICommand<ProjectId>;
