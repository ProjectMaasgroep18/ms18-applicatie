using Maasgroep.Database.Context.Tables.PhotoAlbum;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ms18_applicatie.Controllers.team_d;
using ms18_applicatie.Interfaces;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie_test.team_d.Controllers;

[TestFixture]
public class PhotosControllerTests
{
    private Mock<IPhotoRepository> _mockPhotoRepository;
    private Mock<ILogger<PhotosController>> _mockLogger;
    private PhotosController _controller;

    [SetUp]
    public void Setup()
    {
        _mockPhotoRepository = new Mock<IPhotoRepository>();
        _mockLogger = new Mock<ILogger<PhotosController>>();
        _controller = new PhotosController(_mockPhotoRepository.Object, _mockLogger.Object);

        var httpContext = new DefaultHttpContext();
        var currentUser = new MemberModel
        {
            Id = 123,
            Permissions = new List<string> { "photoAlbum" }
        };
        httpContext.Items["CurrentUser"] = currentUser;
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Test]
    public async Task UploadPhoto_ValidData_ReturnsCreatedResult()
    {
        var uploadModel = new PhotoUploadModel();
        _mockPhotoRepository.Setup(repo => repo.AddPhoto(It.IsAny<long?>(), It.IsAny<bool>(), It.IsAny<PhotoUploadModel>()))
                            .ReturnsAsync(Guid.NewGuid());

        var result = await _controller.UploadPhoto(uploadModel);

        Assert.That(result, Is.InstanceOf<CreatedAtRouteResult>());
        var createdAtRouteResult = result as CreatedAtRouteResult;
        Assert.That(createdAtRouteResult, Is.Not.Null);
        Assert.That(createdAtRouteResult.StatusCode, Is.EqualTo(201));
        Assert.That(createdAtRouteResult.Value, Is.InstanceOf<CreateResponseModel>());
    }

    [Test]
    public async Task UploadPhoto_InvalidData_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("TestError", "This is a test error");

        var uploadModel = new PhotoUploadModel();

        var result = await _controller.UploadPhoto(uploadModel);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UploadPhoto_ErrorOccurs_ReturnsInternalServerError()
    {
        var uploadModel = new PhotoUploadModel();
        _mockPhotoRepository.Setup(repo => repo.AddPhoto(It.IsAny<long?>(), It.IsAny<bool>(), It.IsAny<PhotoUploadModel>()))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.UploadPhoto(uploadModel);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task GetPhoto_ExistingPhotoId_ReturnsPhoto()
    {
        var photoId = Guid.NewGuid();
        var mockPhotoViewModel = new PhotoViewModel() { Id = photoId };
        _mockPhotoRepository.Setup(repo => repo.GetPhotoViewModelById(photoId))
                            .ReturnsAsync(mockPhotoViewModel);

        var result = await _controller.GetPhoto(photoId);

        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.InstanceOf<PhotoViewModel>());
        Assert.That(((PhotoViewModel)okResult.Value).Id, Is.EqualTo(photoId));
    }

    [Test]
    public async Task GetPhoto_NonExistingPhotoId_ReturnsNotFound()
    {
        var photoId = Guid.NewGuid();
        _mockPhotoRepository.Setup(repo => repo.GetPhotoViewModelById(photoId))
                            .ReturnsAsync((PhotoViewModel)null!);

        var result = await _controller.GetPhoto(photoId);

        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetPhoto_ErrorOccurs_ReturnsInternalServerError()
    {
        var photoId = Guid.NewGuid();
        _mockPhotoRepository.Setup(repo => repo.GetPhotoViewModelById(photoId))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetPhoto(photoId);

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task UpdatePhoto_ExistingPhoto_UpdatesAndReturnsNoContent()
    {
        var photoId = Guid.NewGuid();
        var updateModel = new PhotoUpdateModel();
        var existingPhoto = new Photo { UploaderId = 123 };

        _mockPhotoRepository.Setup(repo => repo.GetPhotoById(photoId)).ReturnsAsync(existingPhoto);
        _mockPhotoRepository.Setup(repo => repo.UpdatePhoto(It.IsAny<Photo>())).Returns(Task.CompletedTask);

        var result = await _controller.UpdatePhoto(photoId, updateModel);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task UpdatePhoto_NonExistingPhoto_ReturnsNotFound()
    {
        var photoId = Guid.NewGuid();
        var updateModel = new PhotoUpdateModel();

        _mockPhotoRepository.Setup(repo => repo.GetPhotoById(photoId)).ReturnsAsync((Photo)null!);

        var result = await _controller.UpdatePhoto(photoId, updateModel);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task UpdatePhoto_ForbiddenAccess_ReturnsForbid()
    {
        var photoId = Guid.NewGuid();
        var updateModel = new PhotoUpdateModel();
        var existingPhoto = new Photo { UploaderId = 999 };

        _mockPhotoRepository.Setup(repo => repo.GetPhotoById(photoId)).ReturnsAsync(existingPhoto);

        var result = await _controller.UpdatePhoto(photoId, updateModel);

        Assert.That(result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task UpdatePhoto_InvalidData_ReturnsBadRequest()
    {
        var photoId = Guid.NewGuid();
        _controller.ModelState.AddModelError("TestError", "This is a test error");

        var updateModel = new PhotoUpdateModel();

        var result = await _controller.UpdatePhoto(photoId, updateModel);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdatePhoto_ErrorOccurs_ReturnsInternalServerError()
    {
        var photoId = Guid.NewGuid();
        var updateModel = new PhotoUpdateModel();
        _mockPhotoRepository.Setup(repo => repo.GetPhotoById(photoId)).ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.UpdatePhoto(photoId, updateModel);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task DeletePhoto_ExistingPhoto_DeletesAndReturnsNoContent()
    {
        var photoId = Guid.NewGuid();
        _mockPhotoRepository.Setup(repo => repo.DeletePhoto(photoId)).ReturnsAsync(true);

        var result = await _controller.DeletePhoto(photoId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeletePhoto_NonExistingPhoto_ReturnsNotFound()
    {
        var photoId = Guid.NewGuid();
        _mockPhotoRepository.Setup(repo => repo.DeletePhoto(photoId)).ReturnsAsync(false);

        var result = await _controller.DeletePhoto(photoId);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task DeletePhoto_ErrorOccurs_ReturnsInternalServerError()
    {
        var photoId = Guid.NewGuid();
        _mockPhotoRepository.Setup(repo => repo.DeletePhoto(photoId)).ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.DeletePhoto(photoId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task DownloadPhoto_ExistingPhoto_ReturnsFileContentResult()
    {
        var photoId = Guid.NewGuid();
        var mockPhoto = new Photo
        {
            Id = photoId,
            ImageData = new byte[] { 1, 2, 3, 4, 5 },
            ContentType = "image/jpeg",
            Title = "Test Photo"
        };
        _mockPhotoRepository.Setup(repo => repo.GetPhotoById(photoId)).ReturnsAsync(mockPhoto);

        var result = await _controller.DownloadPhoto(photoId);

        Assert.That(result, Is.InstanceOf<FileStreamResult>());
        var fileResult = result as FileStreamResult;
        Assert.That(fileResult.ContentType, Is.EqualTo(mockPhoto.ContentType));
        Assert.That(fileResult.FileDownloadName, Is.EqualTo(mockPhoto.Title));
    }

    [Test]
    public async Task DownloadPhoto_NonExistingPhoto_ReturnsNotFound()
    {
        var photoId = Guid.NewGuid();
        _mockPhotoRepository.Setup(repo => repo.GetPhotoById(photoId)).ReturnsAsync((Photo)null!);

        var result = await _controller.DownloadPhoto(photoId);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task DownloadPhoto_ErrorOccurs_ReturnsInternalServerError()
    {
        var photoId = Guid.NewGuid();
        _mockPhotoRepository.Setup(repo => repo.GetPhotoById(photoId)).ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.DownloadPhoto(photoId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task ListPhotosInAlbum_ValidParameters_ReturnsPaginatedPhotos()
    {
        var albumId = Guid.NewGuid();
        var mockPhotos = new List<Photo>
    {
        new() {Id= new Guid(), UploadDate = DateTime.Now, ImageData = new byte[] { 1, 2, 3, 4, 5 }, Likes = new List<Like>(){new(), new() }, NeedsApproval = false },
        new() {Id= new Guid(), UploadDate = DateTime.Now, ImageData = new byte[] { 1, 2, 3, 4, 5 }, Likes = new List<Like>(){new(), new() }, NeedsApproval = false }
    };
        var pageNumber = 1;
        var pageSize = 10;
        _mockPhotoRepository.Setup(repo => repo.GetPhotosByAlbumId(albumId, pageNumber, pageSize, false))
                            .ReturnsAsync((mockPhotos, mockPhotos.Count));

        var result = await _controller.ListPhotosInAlbum(albumId, pageNumber, pageSize, false);

        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.InstanceOf<PaginatedResponseModel<PhotoViewModel>>());
        var responseModel = okResult.Value as PaginatedResponseModel<PhotoViewModel>;
        Assert.That(responseModel!.Items!.Count, Is.EqualTo(mockPhotos.Count));
        Assert.That(responseModel.TotalItems, Is.EqualTo(mockPhotos.Count));
        Assert.That(responseModel.CurrentPage, Is.EqualTo(pageNumber));
        Assert.That(responseModel.PageSize, Is.EqualTo(pageSize));
    }

    [Test]
    public async Task ListPhotosInAlbum_InvalidParameters_ReturnsBadRequest()
    {
        var albumId = Guid.NewGuid();
        var pageNumber = 0;
        var pageSize = 10;

        var result = await _controller.ListPhotosInAlbum(albumId, pageNumber, pageSize, false);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task ListPhotosInAlbum_ErrorOccurs_ReturnsInternalServerError()
    {
        var albumId = Guid.NewGuid();
        var pageNumber = 1;
        var pageSize = 10;
        _mockPhotoRepository.Setup(repo => repo.GetPhotosByAlbumId(albumId, pageNumber, pageSize, false))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.ListPhotosInAlbum(albumId, pageNumber, pageSize, false);

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task ListUnapprovedPhotos_ValidParameters_ReturnsPaginatedPhotos()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var mockPhotos = new List<Photo>
    {
        new() {Id= new Guid(), UploadDate = DateTime.Now, ImageData = new byte[] { 1, 2, 3, 4, 5 }, Likes = new List<Like>(){new(), new() }, NeedsApproval = true },
        new() {Id= new Guid(), UploadDate = DateTime.Now, ImageData = new byte[] { 1, 2, 3, 4, 5 }, Likes = new List<Like>(){new(), new() }, NeedsApproval = true }
    };
        _mockPhotoRepository.Setup(repo => repo.GetUnapprovedPhotos(pageNumber, pageSize))
                            .ReturnsAsync((mockPhotos, mockPhotos.Count));

        var result = await _controller.ListUnapprovedPhotos(pageNumber, pageSize);

        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.InstanceOf<PaginatedResponseModel<PhotoViewModel>>());
        var responseModel = okResult.Value as PaginatedResponseModel<PhotoViewModel>;
        Assert.That(responseModel!.Items!.Count, Is.EqualTo(mockPhotos.Count));
        Assert.That(responseModel.TotalItems, Is.EqualTo(mockPhotos.Count));
        Assert.That(responseModel.CurrentPage, Is.EqualTo(pageNumber));
        Assert.That(responseModel.PageSize, Is.EqualTo(pageSize));
    }

    [Test]
    public async Task ListUnapprovedPhotos_InvalidParameters_ReturnsBadRequest()
    {
        var pageNumber = 0;
        var pageSize = 10;

        var result = await _controller.ListUnapprovedPhotos(pageNumber, pageSize);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task ListUnapprovedPhotos_ErrorOccurs_ReturnsInternalServerError()
    {
        var pageNumber = 1;
        var pageSize = 10;
        _mockPhotoRepository.Setup(repo => repo.GetUnapprovedPhotos(pageNumber, pageSize))
                            .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.ListUnapprovedPhotos(pageNumber, pageSize);

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = result.Result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
    }
}

