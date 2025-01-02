namespace Backend.Domains.Project.Domain.DTO;

public class ProjectCreateDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Guid> Users { get; set; } = [];
}
