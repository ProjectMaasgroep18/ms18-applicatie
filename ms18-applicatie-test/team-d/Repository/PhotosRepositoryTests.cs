using Maasgroep.Database.Context.Tables.PhotoAlbum;
using ms18_applicatie.Models.team_d;
using ms18_applicatie.Repository.PhotoAlbum;

namespace ms18_applicatie_test.team_d.Repository;

[TestFixture]
public class PhotosRepositoryTests
{

    [Test]
    public async Task AddPhoto_ValidData_AddsPhoto()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);
        var photoUploadModel = new PhotoUploadModel
        {
            Title = "New Photo",
            ContentType = "image/jpeg",
            ImageData = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 }),
            AlbumId = Guid.NewGuid(),
            TakenOn = DateTime.UtcNow,
            Location = "Test Location"
        };

        var result = await repository.AddPhoto(null, false, photoUploadModel);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));

        var addedPhoto = await context.Photos.FindAsync(result.Value);
        Assert.That(addedPhoto, Is.Not.Null);
        Assert.That(addedPhoto.Title, Is.EqualTo("New Photo"));
    }

    [Test]
    public async Task DeletePhoto_ExistingPhoto_RemovesPhoto()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);
        var photoId = await CreateAndAddMultiplePhotosAsync(context, 1);

        var result = await repository.DeletePhoto(photoId[0]);

        Assert.That(result, Is.True);
        Assert.That(await context.Photos.FindAsync(photoId[0]), Is.Null);
    }

    [Test]
    public async Task GetPhotoViewModelById_ExistingPhoto_ReturnsCorrectPhotoViewModel()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);

        var photoId = await CreateAndAddMultiplePhotosAsync(context, 5);

        var photoViewModel = await repository.GetPhotoViewModelById(photoId[0]);

        Assert.That(photoViewModel, Is.Not.Null);
        Assert.That(photoViewModel.Id, Is.EqualTo(photoId[0]));
        Assert.That(photoViewModel.ImageBase64, Is.EqualTo(Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 })));
        Assert.That(photoViewModel.ContentType, Is.EqualTo("image/jpeg"));
        Assert.That(photoViewModel.NeedsApproval, Is.EqualTo(false));
        Assert.That(photoViewModel.AlbumLocationId, Is.EqualTo(null));
    }

    [Test]
    public async Task UpdatePhoto_WithValidChanges_UpdatesPhotoDetails()
    {
        var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);
        var photoId = await CreateAndAddMultiplePhotosAsync(context, 1);
        var photoToUpdate = await context.Photos.FindAsync(photoId[0]);

        photoToUpdate!.Location = "UpdatedLocation";

        await repository.UpdatePhoto(photoToUpdate);
        var updatedPhoto = await context.Photos.FindAsync(photoId[0]);

        Assert.That(updatedPhoto, Is.Not.Null);
        Assert.That(updatedPhoto.Location, Is.EqualTo(photoToUpdate.Location));
    }

    [Test]
    public async Task GetPhotoById_ExistingPhotoId_ReturnsPhoto()
    {
        var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);
        var photoId = await CreateAndAddMultiplePhotosAsync(context, 5);

        var photo = await repository.GetPhotoById(photoId[0]);

        Assert.That(photo, Is.Not.Null, "Photo should not be null.");
        Assert.That(photo.Id, Is.EqualTo(photoId[0]), "Retrieved photo ID should match the requested ID.");
    }

    [Test]
    public async Task GetPhotoById_NonExistingPhotoId_ReturnsNull()
    {
        var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);
        var nonExistingPhotoId = Guid.NewGuid();

        var photo = await repository.GetPhotoById(nonExistingPhotoId);

        Assert.That(photo, Is.Null, "Photo should be null for a non-existing ID.");
    }

    [Test]
    public async Task GetPhotosByAlbumId_WithPagination_ReturnsCorrectPhotos()
    {
        var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);
        var albumId = Guid.NewGuid();

        await CreateAndAddMultiplePhotosAsync(context, 10, false, albumId);
        await CreateAndAddMultiplePhotosAsync(context, 5, true, albumId);

        int pageNumber = 1;
        int pageSize = 10;
        var (photos, totalCount) = await repository.GetPhotosByAlbumId(albumId, pageNumber, pageSize, showUnapproved: false);

        Assert.That(photos.Count(), Is.EqualTo(10));
        Assert.That(totalCount, Is.EqualTo(10)); 
    }

    [Test]
    public async Task GetUnapprovedPhotos_WithPagination_ReturnsCorrectPhotos()
    {
        var context = TestDbContextFactory.Create();
        var repository = new PhotoRepository(context);

        await CreateAndAddMultiplePhotosAsync(context, 10, false);
        await CreateAndAddMultiplePhotosAsync(context, 10, true);

        int pageNumber = 1;
        int pageSize = 5;
        var (photos, totalCount) = await repository.GetUnapprovedPhotos(pageNumber, pageSize);

        Assert.That(photos.Count(), Is.EqualTo(5));
        Assert.That(totalCount, Is.EqualTo(10));
    }

    private async Task<List<Guid>> CreateAndAddMultiplePhotosAsync(MaasgroepContext context, int numberOfPhotos, bool needsApproval = false, Guid? albumId = null)
    {
        List<Guid> photoIds = new();
        for (int i = 0; i < numberOfPhotos; i++)
        {
            var photo = new Photo
            (
                uploaderId: null,
                imageData: new byte[] { 1, 2, 3, 4, 5 },
                contentType: "image/jpeg",
                needsApproval: needsApproval,
                albumLocationId: albumId
            );

            context.Photos.Add(photo);
        }

        await context.SaveChangesAsync();
        photoIds.AddRange(context.Photos.Select(p => p.Id));
        return photoIds;
    }

}

