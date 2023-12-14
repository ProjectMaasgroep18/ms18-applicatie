using Maasgroep.Database.Context.team_d.Models;
using ms18_applicatie.Repository.team_d;

namespace ms18_applicatie_test.team_d.Repository;
public class AlbumRepositoryTests : BaseTestClass
{
    [Test]
    public async Task AlbumExists_ReturnsCorrect()
    {
        var album = new Album { Id = Guid.NewGuid(), Name = "TestAlbum" };
        Context.Albums.Add(album);
        await Context.SaveChangesAsync();

        var albumRepository = new AlbumRepository(Context);
        var albumDoesExist = await albumRepository.AlbumExists(album.Id);
        var albumDoesNotExist = await albumRepository.AlbumExists(Guid.NewGuid());

        Assert.Multiple(() =>
        {
            Assert.That(albumDoesNotExist, Is.False);
            Assert.That(albumDoesExist, Is.True);
        });
    }

    [Test]
    public async Task CreateAlbum_CreatesCorrect()
    {
        var parentAlbum = new Album { Id = Guid.NewGuid(), Name = "Parent album" };
        var childAlbum = new Album { Id = Guid.NewGuid(), Name = "Child album", ParentAlbumId = parentAlbum.Id };

        var albumRepository = new AlbumRepository(Context);
        var createParentAlbum = await albumRepository.CreateAlbum(parentAlbum);
        var createChildAlbum = await albumRepository.CreateAlbum(childAlbum);

        Assert.Multiple(() =>
        {
            Assert.That(createParentAlbum, Is.Not.Null);
            Assert.That(createChildAlbum, Is.Not.Null);
        });

        Assert.Multiple(() =>
        {
            Assert.That(createParentAlbum.Name, Is.EqualTo(parentAlbum.Name));
            Assert.That(createParentAlbum.Id, Is.EqualTo(parentAlbum.Id));
            Assert.That(createParentAlbum.ParentAlbumId, Is.EqualTo(parentAlbum.ParentAlbumId));

            Assert.That(createChildAlbum.Name, Is.EqualTo(childAlbum.Name));
            Assert.That(createChildAlbum.Id, Is.EqualTo(childAlbum.Id));
            Assert.That(createChildAlbum.ParentAlbumId, Is.EqualTo(childAlbum.ParentAlbumId));
        });

        var parentAlbumInDb = await Context.Albums.FindAsync(parentAlbum.Id);
        var childAlbumInDb = await Context.Albums.FindAsync(childAlbum.Id);

        Assert.Multiple(() =>
        {
            Assert.That(parentAlbumInDb, Is.Not.Null);
            Assert.That(childAlbumInDb, Is.Not.Null);
        });

        Assert.Multiple(() =>
        {
            Assert.That(parentAlbumInDb!.Name, Is.EqualTo(parentAlbum.Name));
            Assert.That(childAlbumInDb!.Name, Is.EqualTo(childAlbum.Name));
        });
    }
}