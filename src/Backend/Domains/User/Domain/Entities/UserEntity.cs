using Backend.Domains.User.Domain.VO;

namespace Backend.Domains.User.Domain.Entities;

public class UserEntity
{
    private UserEntity(UserId id, DateTime createdAt, DateTime? updatedAt, Name firstName, Name lastName, Email email, Phone? phone,
        SsoRole role, string userName, string? hash, bool active, bool isInitialUser)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Role = role;
        UserName = userName;
        Hash = hash;
        Active = active;
        IsInitialUser = isInitialUser;
    }

    public UserId Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public Email Email { get; private set; }
    public Phone? Phone { get; private set; }

    public SsoRole Role { get; private set; }
    public string UserName { get; private set; }
    public string? Hash { get; private set; }
    public bool Active { get; private set; }
    public bool IsInitialUser { get; private set; }

    public string FullName => $"{FirstName.Value} {LastName.Value}";

    public UserEntity WithFirstName(Name firstName)
    {
        FirstName = firstName;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public UserEntity WithLastName(Name lastName)
    {
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public UserEntity WithEmail(Email email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public UserEntity WithPhone(Phone? phone)
    {
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public UserEntity WithUserName(string userName)
    {
        UserName = userName;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public UserEntity WithPassword(string password)
    {
        Hash = BCrypt.Net.BCrypt.HashPassword(password);
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public UserEntity WithActive(bool active)
    {
        Active = active;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public bool ValidatePassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Hash);
    }

    public static UserEntity Create(Name firstName, Name lastName, Email email, Phone? phone, SsoRole role, string userName, bool active = true, bool initialUser = false)
    {
        return new UserEntity(UserId.From(Guid.NewGuid()), DateTime.UtcNow, null, firstName, lastName, email, phone, role, userName,
            null, active, initialUser);
    }
}
