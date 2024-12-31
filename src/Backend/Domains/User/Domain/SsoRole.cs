namespace Backend.Domains.User.Domain;

public enum SsoRole
{
    /// <summary>
    ///     User with administrative privileges. Can access hangfire dashboards from all projects
    /// </summary>
    Developer,

    /// <summary>
    ///     User with administrative privileges.
    /// </summary>
    Administrator,

    /// <summary>
    ///     User which can attack other users to projects
    /// </summary>
    Manager,

    /// <summary>
    ///     User which can only access assigned projects
    /// </summary>
    User,
}
