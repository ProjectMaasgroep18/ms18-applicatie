using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ms18.Application.Repository.TeamD;
using Ms18.Database;
using Ms18.Database.Models.TeamD.PhotoAlbum;
using NUnit.Framework;

namespace Ms18.Application.Test.Repository.TeamD;

public class FolderRepositoryTests : IDisposable
{
    MaasgroepContext _context;

    public FolderRepositoryTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.development.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkNpgsql()
            .BuildServiceProvider();

        var builder = new DbContextOptionsBuilder<MaasgroepContext>();
        builder.UseNpgsql($"UserID=postgres;Password=postgres;Host=localhost;port=5410;Database=TestMaasgroep-{Guid.NewGuid()};Pooling=true")
            .UseInternalServiceProvider(serviceProvider);

        _context = new MaasgroepContext(builder.Options);
        _context.Database.Migrate();
    }

    [Test]
    public async Task FolderExists_ReturnsCorrect()
    {
        var folder = new Folder { Id = Guid.NewGuid(), Name = "TestFolder" };
        _context.Folders.Add(folder);
        await _context.SaveChangesAsync();

        var folderRepository = new FolderRepository(_context);
        var folderDoesExist = await folderRepository.FolderExists(folder.Id);
        var folderDoesNotExist = await folderRepository.FolderExists(Guid.NewGuid());

        Assert.That(folderDoesNotExist, Is.False);
        Assert.That(folderDoesExist, Is.True);
    }


    public void Dispose()
    {
        _context.Database.EnsureDeleted();

    }
}
