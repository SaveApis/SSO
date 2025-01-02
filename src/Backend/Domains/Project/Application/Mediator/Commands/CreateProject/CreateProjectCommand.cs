using Backend.Domains.Project.Domain.DTO;
using Backend.Domains.Project.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.Project.Application.Mediator.Commands.CreateProject;

public record CreateProjectCommand(ProjectCreateDto Dto) : ICommand<ProjectId>;
