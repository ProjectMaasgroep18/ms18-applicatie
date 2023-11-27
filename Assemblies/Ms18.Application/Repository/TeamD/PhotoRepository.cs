using Ms18.Application.Interface.TeamD;
using Ms18.Application.Models.TeamD;
using Ms18.Database;
using Ms18.Database.Models.TeamD.PhotoAlbum;

namespace Ms18.Application.Repository.TeamD;

public class PhotoRepository : IPhotoRepository
{
    private readonly MaasgroepContext _context;

    public PhotoRepository(MaasgroepContext context)
    {
        _context = context;
    }

    public async Task<Photo> AddPhoto(PhotoUploadModel model, long uploaderId)
    {
        // Decode the base64 string to a byte array
        var imageBytes = Convert.FromBase64String(model.ImageBase64);

        // Create a new Photo instance
        var photo = new Photo
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            ImageData = imageBytes,
            ContentType = model.ContentType,
            FolderLocationId = model.FolderLocationId,
            UploadDate = DateTime.UtcNow,
            UploaderId = uploaderId // TODO: Set the uploader's ID based on the authenticated user
        };

        // Add the photo to the database context
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        // Associate tags with the photo if any tag IDs are provided
        if (model.TagIds.Count > 0)
        {
            foreach (var tagId in model.TagIds)
            {
                var photoTag = new PhotoTag
                {
                    PhotoId = photo.Id,
                    TagId = tagId
                };
                _context.PhotoTags.Add(photoTag);
            }
            await _context.SaveChangesAsync();
        }

        return photo;
    }

}

