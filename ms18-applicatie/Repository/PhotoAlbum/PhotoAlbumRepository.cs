using Maasgroep.Database;
using Maasgroep.Database.team_d.Models;
using Microsoft.EntityFrameworkCore;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Repository.PhotoAlbum;

public class PhotoAlbumRepository
{
    private readonly MaasgroepContext _context;

    public PhotoAlbumRepository(MaasgroepContext context)
    {
        _context = context;
    }

    public async Task<bool> AlbumExists(Guid albumId)
    {
        return await _context.Folders.AnyAsync(f => f.Id == albumId);
    }

    public async Task<Folder> CreateFolder(string name, Guid? parentFolderId)
    {
        var folder = new Folder
        {
            Name = name,
            ParentFolderId = parentFolderId
        };

        _context.Folders.Add(folder);
        await _context.SaveChangesAsync();
        return folder;
    }

    public async Task<bool> DeleteFolder(Guid folderId)
    {
        var folder = await _context.Folders.FindAsync(folderId);
        if (folder != null)
        {
            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> RenameFolder(Guid folderId, string newName)
    {
        var folder = await _context.Folders.FindAsync(folderId);
        if (folder != null)
        {
            folder.Name = newName;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> MoveFolder(Guid folderId, Guid newParentFolderId)
    {
        var folder = await _context.Folders.FindAsync(folderId);
        if (folder != null)
        {
            folder.ParentFolderId = newParentFolderId;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
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
        if (model.TagIds != null && model.TagIds.Count > 0)
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

    public async Task<PhotosInDirectoryResponse> GetPhotosInDirectory(
        Guid folderLocationId,
        int page,
        int pageSize)
    {
        var query = _context.Photos
            .Where(p => p.FolderLocationId == folderLocationId);

        var totalCount = await query.CountAsync();

        var photos = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new PhotosInDirectoryResponse
        {
            Photos = photos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return response;
    }

    public async Task<IEnumerable<Folder>> GetFoldersInDirectory(Guid? folderLocationId)
    {
        var query = 
            _context.Folders
            .AsQueryable();

        if (folderLocationId.HasValue)
        {
            query = query.Where(f => f.ParentFolderId == folderLocationId);
        }
        else
        {
            query = query.Where(f => f.ParentFolderId == null);
        }

        var folders = await query.ToListAsync();

        return folders;
    }
}

