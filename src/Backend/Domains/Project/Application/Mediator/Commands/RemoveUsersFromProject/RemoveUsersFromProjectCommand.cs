using Backend.Domains.Project.Domain.VO;
using Backend.Domains.User.Domain.VO;
using SaveApis.Core.Infrastructure.Mediator.Commands;

namespace Backend.Domains.Project.Application.Mediator.Commands.RemoveUsersFromProject;

public record RemoveUsersFromProjectCommand(ProjectId Id, params UserId[] Users) : ICommand<ProjectId>;
