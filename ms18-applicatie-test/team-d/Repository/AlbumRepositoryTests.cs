using Maasgroep.Database.Context.Tables.PhotoAlbum;
using ms18_applicatie.Models.team_d;
using ms18_applicatie.Repository.PhotoAlbum;

namespace ms18_applicatie_test.team_d.Repository;

[TestFixture]
public class AlbumRepositoryTests
{
    [Test]
    public async Task AlbumExists_WithExistingAlbum_ReturnsTrue()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);
        var album = new Album { Name = "Existing Album", ParentAlbumId = null, Year = 2020 };
        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var result = await repository.AlbumExists("Existing Album", null);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AlbumExists_WithNonExistingAlbum_ReturnsFalse()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);

        var result = await repository.AlbumExists("NonExisting Album", null);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AddAlbum_WithValidData_AddsAlbum()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);
        var albumCreateModel = new AlbumCreateModel
        {
            Name = "New Album",
            ParentAlbumId = null,
            Year = 2020
        };

        var result = await repository.AddAlbum(albumCreateModel);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task GetAlbumViewModelById_WithExistingAlbum_ReturnsValidAlbumViewModel()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);
        var album = new Album { Name = "Test Album", ParentAlbumId = null, Year = 2020 };
        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var result = await repository.GetAlbumViewModelById(album.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(album.Id));
        Assert.That(result?.Name, Is.EqualTo("Test Album"));
        Assert.That(result?.Year, Is.EqualTo(2020));
    }

    [Test]
    public async Task GetAlbumById_WithExistingAlbum_ReturnsAlbum()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);
        var album = new Album { Name = "Test Album", ParentAlbumId = null, Year = 2020 };
        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var result = await repository.GetAlbumById(album.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Name, Is.EqualTo("Test Album"));
        Assert.That(result?.ParentAlbumId, Is.Null);
        Assert.That(result?.Year, Is.EqualTo(2020));
    }


    [Test]
    public async Task GetAlbumById_AlbumDoesNotExist_ReturnsNull()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);

        var result = await repository.GetAlbumById(Guid.NewGuid());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAllAlbums_ReturnsAllAlbums()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);

        var album1 = new Album { Name = "Album 1", ParentAlbumId = null, Year = 2020 };
        context.Albums.Add(album1);

        var album2 = new Album { Name = "Album 2", ParentAlbumId = null, Year = 2021 };
        context.Albums.Add(album2);
        await context.SaveChangesAsync();

        context.Albums.Add(new Album { Name = "ChildAlbum 1", ParentAlbumId = album1.Id});
        await context.SaveChangesAsync();

        var albums = await repository.GetAllAlbums();

        Assert.That(albums, Is.Not.Null);
        Assert.That(albums, Has.Count.EqualTo(3));
        Assert.That(albums.Any(a => a.Name == "Album 1"), Is.True);
        Assert.That(albums.Any(a => a.Name == "Album 2"), Is.True);
        Assert.That(albums.Any(a => a.Name == "ChildAlbum 1"), Is.True);
    }


    [Test]
    public async Task UpdateAlbum_WithValidData_UpdatesAlbum()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);

        var album = new Album { Name = "Original Album", Year = 2020 };
        context.Albums.Add(album);
        await context.SaveChangesAsync();

        album.Name = "Updated Album";
        album.Year = 2021;
        await repository.UpdateAlbum(album);

        var updatedAlbum = await context.Albums.FindAsync(album.Id);

        Assert.That(updatedAlbum?.Name, Is.EqualTo("Updated Album"));
        Assert.That(updatedAlbum?.Year, Is.EqualTo(2021));
    }

    [Test]
    public async Task DeleteAlbum_WithExistingAlbum_RemovesAlbum()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);
        var album = new Album { Name = "Album to Delete", Year = 2020 };
        context.Albums.Add(album);
        await context.SaveChangesAsync();

        var deleteSuccess = await repository.DeleteAlbum(album.Id);
        var deletedAlbum = await context.Albums.FindAsync(album.Id);

        Assert.That(deleteSuccess, Is.True);
        Assert.That(deletedAlbum, Is.Null);
    }

    [Test]
    public async Task DeleteAlbum_WhenAlbumDoesNotExist_NoExceptionThrown()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new AlbumRepository(context);

        var nonExistingAlbumId = Guid.NewGuid();
        var deleteSuccess = await repository.DeleteAlbum(nonExistingAlbumId);
        
        Assert.That(deleteSuccess, Is.False);
    }
}