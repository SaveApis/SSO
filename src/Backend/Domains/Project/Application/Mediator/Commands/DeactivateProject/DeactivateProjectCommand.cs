using Backend.Domains.Project.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.Project.Application.Mediator.Commands.DeactivateProject;

public record DeactivateProjectCommand(ProjectId Id) : ICommand<ProjectId>;
