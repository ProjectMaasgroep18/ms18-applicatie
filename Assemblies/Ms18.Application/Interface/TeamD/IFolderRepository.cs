using Ms18.Database.Models.TeamD.PhotoAlbum;

namespace Ms18.Application.Interface.TeamD;

public interface IFolderRepository
{
    Task<bool> FolderExists(Guid albumId);
    Task<Folder> CreateFolder(string name, Guid? parentFolderId);
}

