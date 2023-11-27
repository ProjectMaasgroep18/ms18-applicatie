namespace Ms18.Application.Models.TeamD;

public class CreateFolderModel
{
    public string Name { get; set; } = null!;
    public Guid? ParentFolderId { get; set; }
}

