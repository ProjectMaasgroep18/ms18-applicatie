using Maasgroep.Database.Context.Tables.PhotoAlbum;
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
        _controller.ModelState.Clear();
    }

    [Test]
    public async Task CreateAlbum_ValidData_ReturnsCreatedResult()
    {
        var albumCreateModel = new AlbumCreateModel();
        _mockAlbumRepository.Setup(repo => repo.AlbumExists(It.IsAny<string>(), It.IsAny<Guid?>()))
                            .ReturnsAsync(false);
        _mockAlbumRepository.Setup(repo => repo.AddAlbum(It.IsAny<AlbumCreateModel>()))
                            .ReturnsAsync(Guid.NewGuid());

        var result = await _controller.CreateAlbum(albumCreateModel);

        Assert.That(result.Result, Is.InstanceOf<CreatedAtRouteResult>());
        var createdAtRouteResult = result.Result as CreatedAtRouteResult;
        Assert.That(createdAtRouteResult, Is.Not.Null);
        Assert.That(createdAtRouteResult?.StatusCode, Is.EqualTo(201));
        Assert.That(createdAtRouteResult?.Value, Is.InstanceOf<CreateResponseModel>());
    }

    [Test]
    public async Task CreateAlbum_AlbumExists_ReturnsBadRequest()
    {
        var albumCreateModel = new AlbumCreateModel();
        _mockAlbumRepository.Setup(repo => repo.AlbumExists(It.IsAny<string>(), It.IsAny<Guid?>()))
                            .ReturnsAsync(true);

        var result = await _controller.CreateAlbum(albumCreateModel);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateAlbum_InvalidData_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("TestError", "This is a test error");

        var albumCreateModel = new AlbumCreateModel();

        var result = await _controller.CreateAlbum(albumCreateModel);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateAlbum_ErrorOccurs_ReturnsInternalServerError()
    {
        var albumCreateModel = new AlbumCreateModel();
        _mockAlbumRepository.Setup(repo => repo.AlbumExists(It.IsAny<string>(), It.IsAny<Guid?>()))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.CreateAlbum(albumCreateModel);

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task GetAlbum_ExistingAlbumId_ReturnsAlbum()
    {
        var albumId = Guid.NewGuid();
        var mockAlbumViewModel = new AlbumViewModel();
        _mockAlbumRepository.Setup(repo => repo.GetAlbumViewModelById(albumId))
                            .ReturnsAsync(mockAlbumViewModel);

        var result = await _controller.GetAlbum(albumId);

        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.InstanceOf<AlbumViewModel>());
        var returnValue = okResult.Value as AlbumViewModel;
        Assert.That(returnValue, Is.EqualTo(mockAlbumViewModel));
    }

    [Test]
    public async Task GetAlbum_NonExistingAlbumId_ReturnsNotFound()
    {
        var albumId = Guid.NewGuid();
        _mockAlbumRepository.Setup(repo => repo.GetAlbumViewModelById(albumId))
                            .ReturnsAsync((AlbumViewModel)null!);

        var result = await _controller.GetAlbum(albumId);

        Assert.That(result?.Result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetAlbum_ErrorOccurs_ReturnsInternalServerError()
    {
        var albumId = Guid.NewGuid();
        _mockAlbumRepository.Setup(repo => repo.GetAlbumViewModelById(albumId))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetAlbum(albumId);

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task GetAllAlbums_ReturnsAllAlbums()
    {
        var mockAlbums = new List<AlbumViewModel>
    {
        new (),
        new ()
    };
        _mockAlbumRepository.Setup(repo => repo.GetAllAlbums())
                            .ReturnsAsync(mockAlbums);

        var result = await _controller.GetAllAlbums();

        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.InstanceOf<List<AlbumViewModel>>());
        var returnValue = okResult.Value as List<AlbumViewModel>;
        Assert.That(returnValue, Is.EquivalentTo(mockAlbums));
    }

    [Test]
    public async Task GetAllAlbums_ErrorOccurs_ReturnsInternalServerError()
    {
        _mockAlbumRepository.Setup(repo => repo.GetAllAlbums())
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetAllAlbums();

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task UpdateAlbum_ExistingAlbum_UpdatesAndReturnsNoContent()
    {
        var albumId = Guid.NewGuid();
        var updatedAlbum = new AlbumUpdateModel();
        var existingAlbum = new Album();

        _mockAlbumRepository.Setup(repo => repo.GetAlbumById(albumId))
                            .ReturnsAsync(existingAlbum);
        _mockAlbumRepository.Setup(repo => repo.UpdateAlbum(It.IsAny<Album>()))
                            .Returns(Task.CompletedTask);

        var result = await _controller.UpdateAlbum(albumId, updatedAlbum);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task UpdateAlbum_NonExistingAlbum_ReturnsNotFound()
    {
        var albumId = Guid.NewGuid();
        var updatedAlbum = new AlbumUpdateModel();

        _mockAlbumRepository.Setup(repo => repo.GetAlbumById(albumId))
                            .ReturnsAsync((Album)null!);

        var result = await _controller.UpdateAlbum(albumId, updatedAlbum);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UpdateAlbum_InvalidData_ReturnsBadRequest()
    {
        var albumId = Guid.NewGuid();
        _controller.ModelState.AddModelError("TestError", "This is a test error");

        var updatedAlbum = new AlbumUpdateModel();

        var result = await _controller.UpdateAlbum(albumId, updatedAlbum);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateAlbum_ErrorOccurs_ReturnsInternalServerError()
    {
        var albumId = Guid.NewGuid();
        var updatedAlbum = new AlbumUpdateModel();

        _mockAlbumRepository.Setup(repo => repo.GetAlbumById(albumId))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.UpdateAlbum(albumId, updatedAlbum);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task DeleteAlbum_ExistingAlbum_DeletesAndReturnsNoContent()
    {
        var albumId = Guid.NewGuid();
        _mockAlbumRepository.Setup(repo => repo.DeleteAlbum(albumId)).ReturnsAsync(true);

        var result = await _controller.DeleteAlbum(albumId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteAlbum_NonExistingAlbum_ReturnsNotFound()
    {
        var albumId = Guid.NewGuid();
        _mockAlbumRepository.Setup(repo => repo.DeleteAlbum(albumId)).ReturnsAsync(false);

        var result = await _controller.DeleteAlbum(albumId);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task DeleteAlbum_ErrorOccurs_ReturnsInternalServerError()
    {
        var albumId = Guid.NewGuid();
        _mockAlbumRepository.Setup(repo => repo.DeleteAlbum(albumId)).ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.DeleteAlbum(albumId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }
}