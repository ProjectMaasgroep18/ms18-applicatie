using Ms18.Application.Models.TeamD;
using Ms18.Database.Models.TeamD.PhotoAlbum;

namespace Ms18.Application.Interface.TeamD;

public interface IPhotoRepository
{
    Task<Photo> AddPhoto(PhotoUploadModel model, long uploaderId);
}

