using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Maasgroep.Repository.PhotoAlbum;

namespace ms18_applicatie_test.team_d.Repository;
public class AlbumRepositoryTests : BaseTestClass
{
    [Test]
    public async Task AlbumExists_ReturnsCorrect()
    {        
        var parentAlbum = new Album { Id = Guid.NewGuid(), Name = "TestParentAlbum"};
        var album = new Album { Id = Guid.NewGuid(), Name = "TestAlbum", ParentAlbumId = parentAlbum.Id};

        Context.Albums.Add(parentAlbum);
        Context.Albums.Add(album);

        await Context.SaveChangesAsync();

        var albumRepository = new AlbumRepository(Context);
        var albumDoesExist = await albumRepository.AlbumExists("TestAlbum", album.ParentAlbumId);
        var albumDoesNotExist = await albumRepository.AlbumExists("TestAlbum", Guid.NewGuid());

        Assert.Multiple(() =>
        {
            Assert.That(albumDoesNotExist, Is.False);
            Assert.That(albumDoesExist, Is.True);
        });
    }

    [Test]
    public async Task AddsAlbum_AddsCorrect()
    {
        var parentAlbum = new Album { Id = Guid.NewGuid(), Name = "Parent album" };
        var childAlbum = new Album { Id = Guid.NewGuid(), Name = "Child album", ParentAlbumId = parentAlbum.Id };

        var albumRepository = new AlbumRepository(Context);
        await albumRepository.AddAlbum(parentAlbum);
        await albumRepository.AddAlbum(childAlbum);

        var retrievedParentAlbum = await albumRepository.GetAlbumById(parentAlbum.Id);
        var retrievedChildAlbum = await albumRepository.GetAlbumById(childAlbum.Id);
        Assert.Multiple(() =>
        {
            Assert.That(retrievedParentAlbum, Is.Not.Null);
            Assert.That(retrievedChildAlbum, Is.Not.Null);
            Assert.That(retrievedParentAlbum?.Name, Is.EqualTo("Parent album"));
            Assert.That(retrievedChildAlbum?.Name, Is.EqualTo("Child album"));
            Assert.That(retrievedParentAlbum?.ChildAlbums, Is.Not.Null);
            Assert.That(retrievedParentAlbum?.ChildAlbums, Contains.Item(retrievedChildAlbum));


        });
    }
}