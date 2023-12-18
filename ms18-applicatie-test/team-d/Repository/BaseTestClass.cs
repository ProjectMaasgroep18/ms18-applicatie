namespace ms18_applicatie_test.team_d.Repository;

public class BaseTestClass : IDisposable
{
    protected readonly MaasgroepContext Context;

    protected BaseTestClass()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionBuilder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
        {
            Database = $"Test-Maasgroep-{Guid.NewGuid()}"
        };
        var newConnectionString = connectionBuilder.ConnectionString;

        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkNpgsql()
            .BuildServiceProvider();

        var builder = new DbContextOptionsBuilder<MaasgroepContext>();
        builder.UseNpgsql(newConnectionString)
            .UseInternalServiceProvider(serviceProvider);

        Context = new MaasgroepContext(builder.Options);
        Context.Database.Migrate();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
    }
}
