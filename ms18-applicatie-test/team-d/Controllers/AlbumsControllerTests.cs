using Maasgroep.Database.Context.team_d.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ms18_applicatie.Controllers.team_d;
using ms18_applicatie.Interfaces.team_d;

namespace ms18_applicatie_test.team_d.Controllers;

public class AlbumsControllerTests
{
    private Mock<IAlbumRepository> _mockAlbumRepository;
    private Mock<IPhotoRepository> _mockPhotoRepository;
    private Mock<ILogger<AlbumsController>> _mockLogger;

    private AlbumsController _controller;

    [SetUp]
    public void Setup()
    {
        // Arrange
        _mockAlbumRepository = new Mock<IAlbumRepository>();
        _mockPhotoRepository = new Mock<IPhotoRepository>();
        _mockLogger = new Mock<ILogger<AlbumsController>>();

        _controller = new AlbumsController(_mockPhotoRepository.Object, _mockAlbumRepository.Object, _mockLogger.Object);
    }


    [Test]
    public async Task CreateAlbum_ReturnsCreatedResult()
    {
        var albumName = "New Album";
        var parentAlbumId = new Guid();
        var model = new Album { Name = albumName, ParentAlbumId = parentAlbumId };
        var expectedAlbum = new Album { Name = albumName, ParentAlbumId = parentAlbumId };

        _mockAlbumRepository.Setup(repo => repo.CreateAlbum(model))
            .ReturnsAsync(expectedAlbum);

        var result = await _controller.CreateAlbum(model);

        // Assert
        Assert.IsInstanceOf<CreatedResult>(result);
        var createdResult = result as CreatedResult;
        Assert.AreEqual(201, createdResult.StatusCode);
        Assert.AreEqual(expectedAlbum, createdResult.Value);

        _mockAlbumRepository.Verify(repo => repo.CreateAlbum(model), Times.Once);
    }
}