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

    [Test]
    public async Task CreateFolder_CreatesCorrect()
    {
        var parentFolder = new Folder { Id = Guid.NewGuid(), Name = "Parent Folder" };
        var childFolder = new Folder { Id = Guid.NewGuid(), Name = "Child Folder", ParentFolderId = parentFolder.Id };

        var folderRepository = new FolderRepository(Context);
        var createParentFolder = await folderRepository.CreateFolder(parentFolder);
        var createChildFolder = await folderRepository.CreateFolder(childFolder);

        Assert.IsNotNull(createParentFolder);
        Assert.IsNotNull(createChildFolder);

        Assert.AreEqual(parentFolder.Name, createParentFolder.Name);
        Assert.AreEqual(parentFolder.Id, createParentFolder.Id);
        Assert.AreEqual(parentFolder.ParentFolderId, createParentFolder.ParentFolderId);

        Assert.AreEqual(childFolder.Name, createChildFolder.Name);
        Assert.AreEqual(childFolder.Id, createChildFolder.Id);
        Assert.AreEqual(childFolder.ParentFolderId, createChildFolder.ParentFolderId);

        var parentFolderInDb = await Context.Folders.FindAsync(parentFolder.Id);
        var childFolderInDb = await Context.Folders.FindAsync(childFolder.Id);

        Assert.IsNotNull(parentFolderInDb);
        Assert.IsNotNull(childFolderInDb);

        Assert.AreEqual(parentFolder.Name, parentFolderInDb!.Name);
        Assert.AreEqual(childFolder.Name, childFolderInDb!.Name);
    }
}
