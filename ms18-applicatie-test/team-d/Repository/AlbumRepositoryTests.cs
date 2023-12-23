//using Maasgroep.Database.Context.Tables.PhotoAlbum;
//using ms18_applicatie.Repository.PhotoAlbum;

//namespace ms18_applicatie_test.team_d.Repository;

//[TestFixture]
//public class AlbumRepositoryTests
//{
//    [Test]
//    public async Task AddAlbum_WhenCalledWithValidData_AddsAlbumToDatabase()
//    {
//        await using var context = TestDbContextFactory.Create();
//        var repository = new AlbumRepository(context);
//        var newAlbum = new Album { Name = "New Album", ParentAlbumId = Guid.NewGuid(), Year = 2020 };

//        var result = await repository.AddAlbum(newAlbum);

//        Assert.That(result, Is.Not.Null);
//        Assert.That(result, Is.Not.EqualTo(Guid.Empty));

//        var addedAlbum = await context.Albums.FindAsync(result);
//        Assert.That(addedAlbum, Is.Not.Null);
//        Assert.That(addedAlbum?.Name, Is.EqualTo("New Album"));
//    }

//    [Test]
//    public async Task GetAlbumById_AlbumExists_ReturnsAlbum()
//    {
//        await using var context = TestDbContextFactory.Create();
//        var repository = new AlbumRepository(context);
//        var newAlbum = new Album { Name = "Test Album", ParentAlbumId = null, Year = 2020 };
//        context.Albums.Add(newAlbum);
//        await context.SaveChangesAsync();

//        var childAlbum = new Album { Name = "Child Album", ParentAlbumId = newAlbum.Id, Year = 2020 };
//        context.Albums.Add(childAlbum);
//        await context.SaveChangesAsync();

//        var grandChildAlbum = new Album { Name = "GrandChildAlbum", ParentAlbumId = childAlbum.Id, Year = 2020 };
//        context.Albums.Add(grandChildAlbum);
//        await context.SaveChangesAsync();

//        var result = await repository.GetAlbumById(newAlbum.Id);

//        Assert.That(result, Is.Not.Null);
//        Assert.That(result?.Id, Is.EqualTo(newAlbum.Id));
//        Assert.That(result?.Name, Is.EqualTo("Test Album"));
//    }

//    [Test]
//    public async Task GetAlbumById_AlbumDoesNotExist_ReturnsNull()
//    {
//        await using var context = TestDbContextFactory.Create();
//        var repository = new AlbumRepository(context);

//        var result = await repository.GetAlbumById(Guid.NewGuid());

//        Assert.That(result, Is.Null);
//    }

//    [Test]
//    public async Task GetAllAlbums_ReturnsAllAlbums()
//    {
//        await using var context = TestDbContextFactory.Create();
//        var repository = new AlbumRepository(context);

//        context.Albums.Add(new Album { Name = "Album 1", ParentAlbumId = null, Year = 2020 });
//        context.Albums.Add(new Album { Name = "Album 2", ParentAlbumId = null, Year = 2021 });
//        await context.SaveChangesAsync();

//        var albums = await repository.GetAllAlbums();

//        Assert.That(albums, Is.Not.Null);
//        Assert.That(albums, Has.Count.EqualTo(2));
//        Assert.That(albums.Any(a => a.Name == "Album 1"), Is.True);
//        Assert.That(albums.Any(a => a.Name == "Album 2"), Is.True);
//    }

//    [Test]
//    public async Task UpdateAlbum_WhenCalled_ChangesArePersisted()
//    {
//        await using var context = TestDbContextFactory.Create();
//        var repository = new AlbumRepository(context);

//        var album = new Album { Name = "Original Album", Year = 2020 };
//        context.Albums.Add(album);
//        await context.SaveChangesAsync();

//        album.Name = "Updated Album";
//        album.Year = 2021;
//        await repository.UpdateAlbum(album);

//        var updatedAlbum = await context.Albums.FindAsync(album.Id);
//        Assert.That(updatedAlbum, Is.Not.Null);
//        Assert.That(updatedAlbum.Name, Is.EqualTo("Updated Album"));
//        Assert.That(updatedAlbum.Year, Is.EqualTo(2021));
//    }

//    [Test]
//    public async Task DeleteAlbum_WhenAlbumExists_RemovesAlbum()
//    {
//        await using var context = TestDbContextFactory.Create();
//        var repository = new AlbumRepository(context);

//        var album = new Album { Name = "Album to Delete", Year = 2020 };
//        context.Albums.Add(album);
//        await context.SaveChangesAsync();

//        await repository.DeleteAlbum(album.Id);

//        var deletedAlbum = await context.Albums.FindAsync(album.Id);
//        Assert.That(deletedAlbum, Is.Null);
//    }

//    [Test]
//    public async Task DeleteAlbum_WhenAlbumDoesNotExist_NoExceptionThrown()
//    {
//        await using var context = TestDbContextFactory.Create();
//        var repository = new AlbumRepository(context);

//        var nonExistingAlbumId = Guid.NewGuid();
//        Assert.DoesNotThrowAsync(async () => await repository.DeleteAlbum(nonExistingAlbumId));
//    }
//}