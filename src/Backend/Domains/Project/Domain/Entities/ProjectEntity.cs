using Backend.Domains.Common.Domain.VO;
using Backend.Domains.Project.Domain.VO;
using Backend.Domains.User.Domain.Entities;

namespace Backend.Domains.Project.Domain.Entities;

public class ProjectEntity
{
    private ProjectEntity(ProjectId id, DateTime createdAt, DateTime? updatedAt, Name name, Description description, bool active)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Name = name;
        Description = description;
        Active = active;
    }

    public ProjectId Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public bool Active { get; private set; }

    public virtual ICollection<UserEntity> Users { get; set; } = [];

    public ProjectEntity WithName(Name name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public ProjectEntity WithDescription(Description description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public ProjectEntity Activate()
    {
        Active = true;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public ProjectEntity Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public ProjectEntity AddUser(UserEntity user)
    {
        if (Users.Contains(user))
        {
            return this;
        }

        Users.Add(user);
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public ProjectEntity AddUsers(params UserEntity[] users)
    {
        foreach (var user in users)
        {
            AddUser(user);
        }

        return this;
    }

    public ProjectEntity RemoveUser(UserEntity user)
    {
        if (!Users.Contains(user))
        {
            return this;
        }

        Users.Remove(user);
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public ProjectEntity RemoveUsers(params UserEntity[] users)
    {
        foreach (var user in users)
        {
            RemoveUser(user);
        }

        return this;
    }

    public static ProjectEntity Create(ProjectId id, Name name, Description description, params UserEntity[] initialUsers)
    {
        var entity = new ProjectEntity(id, DateTime.UtcNow, null, name, description, true);
        entity.AddUsers(initialUsers);

        return entity;
    }
}
