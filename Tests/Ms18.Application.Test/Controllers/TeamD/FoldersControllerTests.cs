using Microsoft.AspNetCore.Mvc;
using Moq;
using Ms18.Application.Controllers.TeamD;
using Ms18.Application.Interface.TeamD;
using Ms18.Application.Models.TeamD;
using Ms18.Database.Models.TeamD.PhotoAlbum;
using NUnit.Framework;

namespace Ms18.Application.Test.Controllers.TeamD;

public class FoldersControllerTests
{
    private Mock<IFolderRepository> _mockFolderRepository;
    private Mock<IPhotoRepository> _mockPhotoRepository;
    private FoldersController _controller;

    [SetUp]
    public void Setup()
    {
        // Arrange
        _mockFolderRepository = new Mock<IFolderRepository>();
        _mockPhotoRepository = new Mock<IPhotoRepository>();

        _controller = new FoldersController(_mockPhotoRepository.Object, _mockFolderRepository.Object);
    }


    [Test]
    public async Task CreateFolder_ReturnsCreatedResult()
    {
        var folderName = "New Folder";
        var parentFolderId = new Guid();
        var model = new CreateFolderModel { Name = folderName, ParentFolderId = parentFolderId};
        var expectedFolder = new Folder() { Name = folderName, ParentFolderId = parentFolderId };

        _mockFolderRepository.Setup(repo => repo.CreateFolder(model.Name, model.ParentFolderId))
            .ReturnsAsync(expectedFolder);

        var result = await _controller.CreateFolder(model);

        // Assert
        Assert.IsInstanceOf<CreatedResult>(result);
        var createdResult = result as CreatedResult;
        Assert.AreEqual(201, createdResult.StatusCode);
        Assert.AreEqual(expectedFolder, createdResult.Value); 

        _mockFolderRepository.Verify(repo => repo.CreateFolder(folderName, parentFolderId), Times.Once);
    }
}