namespace ms18_applicatie_test.team_d;

public static class TestDbContextFactory
{
    public static MaasgroepContext Create()
    {
        var options = new DbContextOptionsBuilder<MaasgroepContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new MaasgroepContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}

