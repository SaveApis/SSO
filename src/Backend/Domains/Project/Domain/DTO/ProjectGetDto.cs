using Backend.Domains.User.Domain.DTO;

namespace Backend.Domains.Project.Domain.DTO;

public class ProjectGetDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Active { get; set; }

    public IEnumerable<UserGetDto> Users { get; set; } = [];
}
