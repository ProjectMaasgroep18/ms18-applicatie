namespace Ms18.Application.Models.TeamD;
public class PhotoUploadModel
{
    public string Title { get; set; } = null!;
    public string ImageBase64 { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public Guid FolderLocationId { get; set; }
    public List<Guid> TagIds { get; set; } = null!;
}

