using Microsoft.EntityFrameworkCore;
using SaveApis.Core.Infrastructure.Persistence.Sql.Factories;

namespace Backend.Persistence.Sql.Factories;

public class UserContextFactory(IConfiguration configuration) : BaseDbFactory<DataContext>(configuration)
{
    public UserContextFactory() : this(new ConfigurationBuilder().AddInMemoryCollection().Build())
    {
    }

    protected override DataContext Create(DbContextOptionsBuilder<DataContext> builder)
    {
        return new DataContext(builder.Options);
    }
}
