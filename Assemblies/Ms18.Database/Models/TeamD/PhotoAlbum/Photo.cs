using Ms18.Database.Models.TeamC.Admin;

namespace Ms18.Database.Models.TeamD.PhotoAlbum;
public record Photo
{
    public Guid Id { get; set; }

    public long UploaderId { get; set; }
    public Member Uploader { get; set; } = null!;
    public DateTime UploadDate { get; set; }

    public string Title { get; set; } = null!;
    public byte[] ImageData { get; set; } = null!;
    public string ContentType { get; set; } = null!;

    public Guid FolderLocationId { get; set; }
    public Folder FolderLocation { get; set; } = null!;

    public IEnumerable<PhotoTag> PhotoTags { get; set; } = null!;
    public IEnumerable<Like> Likes { get; set; } = null!;
}
