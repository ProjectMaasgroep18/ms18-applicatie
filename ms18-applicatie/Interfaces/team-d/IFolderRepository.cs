using Maasgroep.Database.team_d.Models;

namespace ms18_applicatie.Interfaces.team_d;

public interface IFolderRepository
{
    Task<bool> FolderExists(Guid albumId);
    Task<Folder> CreateFolder(Folder folder);
}


