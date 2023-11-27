using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ms18.Application.Controllers.TeamD;
using Ms18.Application.Interface.TeamD;
using Ms18.Database.Models.TeamD.PhotoAlbum;
using NUnit.Framework;

namespace Ms18.Application.Test.Controllers.TeamD;

public class FoldersControllerTests
{
    private Mock<IFolderRepository> _mockFolderRepository;
    private Mock<IPhotoRepository> _mockPhotoRepository;
    private Mock<ILogger<FoldersController>> _mockLogger;

    private FoldersController _controller;

    [SetUp]
    public void Setup()
    {
        // Arrange
        _mockFolderRepository = new Mock<IFolderRepository>();
        _mockPhotoRepository = new Mock<IPhotoRepository>();
        _mockLogger = new Mock<ILogger<FoldersController>>();

        _controller = new FoldersController(_mockPhotoRepository.Object, _mockFolderRepository.Object, _mockLogger.Object);
    }


    [Test]
    public async Task CreateFolder_ReturnsCreatedResult()
    {
        var folderName = "New Folder";
        var parentFolderId = new Guid();
        var model = new Folder { Name = folderName, ParentFolderId = parentFolderId};
        var expectedFolder = new Folder { Name = folderName, ParentFolderId = parentFolderId };

        _mockFolderRepository.Setup(repo => repo.CreateFolder(model))
            .ReturnsAsync(expectedFolder);

        var result = await _controller.CreateFolder(model);

        // Assert
        Assert.IsInstanceOf<CreatedResult>(result);
        var createdResult = result as CreatedResult;
        Assert.AreEqual(201, createdResult.StatusCode);
        Assert.AreEqual(expectedFolder, createdResult.Value); 

        _mockFolderRepository.Verify(repo => repo.CreateFolder(model), Times.Once);
    }
}