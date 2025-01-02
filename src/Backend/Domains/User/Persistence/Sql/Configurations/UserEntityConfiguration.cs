using Backend.Domains.Common.Domain.VO;
using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.Entities;
using Backend.Domains.User.Domain.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Domains.User.Persistence.Sql.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasConversion<UserId.EfCoreValueConverter>();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired(false);

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50).HasConversion<Name.EfCoreValueConverter>();
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(50).HasConversion<Name.EfCoreValueConverter>();
        builder.Property(e => e.Email).IsRequired().HasMaxLength(150).HasConversion<Email.EfCoreValueConverter>();
        builder.Property(e => e.Phone).IsRequired(false).HasConversion<Phone.EfCoreValueConverter>();

        builder.Property(e => e.Role).HasConversion(role => role.ToString(), s => Enum.Parse<SsoRole>(s));
        builder.Property(e => e.UserName).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Hash).IsRequired(false).HasMaxLength(64);
        builder.Property(e => e.Active).IsRequired();
        builder.Property(e => e.IsInitialUser).IsRequired();

        builder.HasIndex(e => new { e.FirstName, e.LastName });
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.UserName).IsUnique();

        builder.HasMany(e => e.Projects).WithMany(e => e.Users);
    }
}
