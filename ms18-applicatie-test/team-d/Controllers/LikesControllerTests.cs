using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ms18_applicatie.Controllers.team_d;
using ms18_applicatie.Interfaces;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie_test.team_d.Controllers;

[TestFixture]
public class LikesControllerTests
{
    private Mock<ILikesRepository> _mockLikesRepository;
    private Mock<ILogger<LikesController>> _mockLogger;
    private LikesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockLikesRepository = new Mock<ILikesRepository>();
        _mockLogger = new Mock<ILogger<LikesController>>();
        _controller = new LikesController(_mockLikesRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetAllLikesForPhoto_WithValidPhotoId_ReturnsLikes()
    {
        var photoId = Guid.NewGuid();
        var mockLikes = new List<LikeViewModel>
        {

        };
        _mockLikesRepository.Setup(repo => repo.GetAllLikesForPhoto(photoId))
                            .ReturnsAsync(mockLikes);

        var result = await _controller.GetAllLikesForPhoto(photoId);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnValue = okResult.Value as IEnumerable<LikeViewModel>;
        Assert.That(returnValue?.Count(), Is.EqualTo(mockLikes.Count));

    }

    [Test]
    public async Task GetAllLikesForPhoto_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        var photoId = Guid.NewGuid();
        _mockLikesRepository.Setup(repo => repo.GetAllLikesForPhoto(photoId))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetAllLikesForPhoto(photoId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task AddLike_NewLike_ReturnsCreated()
    {
        var photoId = Guid.NewGuid();
        var userId = 123L;
        _mockLikesRepository.Setup(repo => repo.GetLike(photoId, userId))
                           .ReturnsAsync((Like)null!); 
        _mockLikesRepository.Setup(repo => repo.AddLike(It.IsAny<Like>()))
                           .ReturnsAsync(Guid.NewGuid());

        var result = await _controller.AddLike(photoId, userId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task AddLike_ExistingLike_ReturnsConflict()
    {
        var photoId = Guid.NewGuid();
        var userId = 123L;
        _mockLikesRepository.Setup(repo => repo.GetLike(photoId, userId))
                           .ReturnsAsync(new Like());


        var result = await _controller.AddLike(photoId, userId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(409));
    }

    [Test]
    public async Task AddLike_ErrorOccurs_ReturnsInternalServerError()
    {
        var photoId = Guid.NewGuid();
        var userId = 123L;
        _mockLikesRepository.Setup(repo => repo.GetLike(photoId, userId))
                           .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.AddLike(photoId, userId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task DeleteLike_ExistingLike_DeletesAndReturnsNoContent()
    {

        var photoId = Guid.NewGuid();
        var userId = 123L;
        var mockLike = new Like { Id = Guid.NewGuid(), PhotoId = photoId, MemberId = userId };
        _mockLikesRepository.Setup(repo => repo.GetLike(photoId, userId)).ReturnsAsync(mockLike);
        _mockLikesRepository.Setup(repo => repo.DeleteLike(mockLike.Id)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteLike(photoId, userId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteLike_NonExistingLike_ReturnsNotFound()
    {
        var photoId = Guid.NewGuid();
        var userId = 123L;
        _mockLikesRepository.Setup(repo => repo.GetLike(photoId, userId)).ReturnsAsync((Like)null!);

        var result = await _controller.DeleteLike(photoId, userId);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task DeleteLike_ErrorOccurs_ReturnsInternalServerError()
    {
        var photoId = Guid.NewGuid();
        var userId = 123L;
        _mockLikesRepository.Setup(repo => repo.GetLike(photoId, userId)).ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.DeleteLike(photoId, userId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task GetTopLikedPhotos_WithValidParameters_ReturnsPhotos()
    {
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        var topCount = 5;
        var mockPhotos = new List<PhotoViewModel>
        {
            new PhotoViewModel()
        };
        _mockLikesRepository.Setup(repo => repo.GetTopLikedPhotos(startDate, endDate, topCount))
                            .ReturnsAsync(mockPhotos);

        var result = await _controller.GetTopLikedPhotos(startDate, endDate, topCount);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnValue = okResult.Value as IEnumerable<PhotoViewModel>;
        Assert.That(returnValue?.Count(), Is.EqualTo(mockPhotos.Count));
    }

    [Test]
    public async Task GetTopLikedPhotos_NoPhotosFound_ReturnsNotFound()
    {
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        var topCount = 5;
        _mockLikesRepository.Setup(repo => repo.GetTopLikedPhotos(startDate, endDate, topCount))
                            .ReturnsAsync(new List<PhotoViewModel>());

        var result = await _controller.GetTopLikedPhotos(startDate, endDate, topCount);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetTopLikedPhotos_ErrorOccurs_ReturnsInternalServerError()
    {
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        var topCount = 5;
        _mockLikesRepository.Setup(repo => repo.GetTopLikedPhotos(startDate, endDate, topCount))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetTopLikedPhotos(startDate, endDate, topCount);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }
}
