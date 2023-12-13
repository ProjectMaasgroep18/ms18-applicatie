using Maasgroep.Database.Context;
using Maasgroep.Database.team_d.Models;
using ms18_applicatie.Interfaces.team_d;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Repository.team_d;

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

