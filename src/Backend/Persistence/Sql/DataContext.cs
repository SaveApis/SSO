using Backend.Domains.User.Domain.Entities;
using Backend.Persistence.Sql.Configurations;
using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.Sql;

namespace Backend.Persistence.Sql;

public class DataContext(DbContextOptions options) : BaseDbContext(options)
{
    protected override string Schema => "sso";

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
    }
}
