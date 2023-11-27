using Ms18.Application.Repository.TeamD;
using Ms18.Database.Models.TeamD.PhotoAlbum;
using NUnit.Framework;

namespace Ms18.Application.Test.Repository.TeamD;

public class FolderRepositoryTests : BaseTestClass
{
    [Test]
    public async Task FolderExists_ReturnsCorrect()
    {
        var folder = new Folder { Id = Guid.NewGuid(), Name = "TestFolder" };
        Context.Folders.Add(folder);
        await Context.SaveChangesAsync();

        var folderRepository = new FolderRepository(Context);
        var folderDoesExist = await folderRepository.FolderExists(folder.Id);
        var folderDoesNotExist = await folderRepository.FolderExists(Guid.NewGuid());

        Assert.That(folderDoesNotExist, Is.False);
        Assert.That(folderDoesExist, Is.True);
    }
}
