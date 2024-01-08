using Maasgroep.Database.Context.Tables.PhotoAlbum;
using ms18_applicatie.Models.team_d;

namespace ms18_applicatie.Interfaces;

public interface IPhotoRepository
{
    Task<Guid?> AddPhoto(long? uploaderId, bool needsApproval, PhotoUploadModel photoUploadModel);
    Task<PhotoViewModel?> GetPhotoViewModelById(Guid id);
    Task<Photo?> GetPhotoById(Guid id);
    Task UpdatePhoto(Photo photo);
    Task<bool> DeletePhoto(Guid id);
    Task<(IEnumerable<Photo> Photos, int TotalCount)> GetPhotosByAlbumId(Guid albumId, int pageNumber, int pageSize, bool showUnapproved);
    Task<(IEnumerable<Photo> Photos, int TotalCount)> GetUnapprovedPhotos(int pageNumber, int pageSize);
}

