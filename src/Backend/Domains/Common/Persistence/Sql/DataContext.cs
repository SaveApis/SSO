using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.Project.Persistence.Sql.Configurations;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Persistence.Sql.Configurations;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.Sql;

namespace Backend.Domains.Common.Persistence.Sql;

public class DataContext(DbContextOptions options) : BaseDbContext(options)
{
    protected override string Schema => "sso";

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
    }
}
