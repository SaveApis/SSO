namespace Backend.Domains.User.Domain.DTO;

public class UserGetDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public SsoRole Role { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool Active { get; set; }
}
