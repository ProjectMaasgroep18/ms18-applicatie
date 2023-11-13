namespace ms18_applicatie.Models.team_d;

public class CreateFolderModel
{
    public string Name { get; set; } = null!;
    public Guid? ParentFolderId { get; set; }
}

