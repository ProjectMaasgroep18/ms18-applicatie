using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ms18_applicatie.Controllers.team_d;
using ms18_applicatie.Interfaces;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie_test.team_d.Controllers;

[TestFixture]
public class AlbumsControllerTests
{
    private Mock<IAlbumRepository> _mockAlbumRepository;
    private Mock<ILogger<AlbumsController>> _mockLogger;

    private AlbumsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockAlbumRepository = new Mock<IAlbumRepository>();
        _mockLogger = new Mock<ILogger<AlbumsController>>();

        _controller = new AlbumsController(_mockAlbumRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task CreateAlbum_WithValidData_ReturnsCreatedResult()
    {
        var albumCreateModel = new AlbumCreateModel { Name = "New Album", ParentAlbumId = null, Year = 2020 };
        var newAlbumId = Guid.NewGuid();

        _mockAlbumRepository.Setup(repo => repo.AlbumExists(It.IsAny<string>(), It.IsAny<Guid?>()))
            .ReturnsAsync(false);
        _mockAlbumRepository.Setup(repo => repo.AddAlbum(albumCreateModel))
            .ReturnsAsync(newAlbumId);

        var result = await _controller.CreateAlbum(albumCreateModel);

        Assert.That(result.Result, Is.InstanceOf<CreatedAtRouteResult>());

        var createdResult = result.Result as CreatedAtRouteResult;
        Assert.That(createdResult?.StatusCode, Is.EqualTo(201));

        var createdResponseModel = createdResult?.Value as CreateResponseModel;
        Assert.That(createdResponseModel, Is.Not.Null);
        Assert.That(createdResponseModel?.Id, Is.EqualTo(newAlbumId));

        _mockAlbumRepository.Verify(repo => repo.AddAlbum(albumCreateModel), Times.Once);
    }
}