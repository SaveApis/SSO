using Backend.Domains.Common.Domain.VO;
using Backend.Domains.Project.Domain.Entities;
using Backend.Domains.Project.Domain.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Domains.Project.Persistence.Sql.Configurations;

public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasConversion<ProjectId.EfCoreValueConverter>();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired(false);

        builder.Property(e => e.Name).IsRequired().HasConversion<Name.EfCoreValueConverter>();
        builder.Property(e => e.Description).IsRequired().HasConversion<Description.EfCoreValueConverter>();

        builder.HasMany(e => e.Users).WithMany(e => e.Projects);

        builder.HasIndex(e => e.Name).IsUnique();
    }
}
