using Maasgroep.Database.Members;

namespace Maasgroep.Database.Context.team_d.Models;

public class Photo
{
    public Guid Id { get; set; }

    public long UploaderId { get; set; }
    public Member Uploader { get; set; } = null!;
    public DateTime UploadDate { get; set; }

    public string Title { get; set; } = null!;
    public byte[] ImageData { get; set; } = null!;
    public string ContentType { get; set; } = null!;

    public Guid AlbumLocationId { get; set; }
    public Album AlbumLocation { get; set; } = null!;

    public IEnumerable<Like> Likes { get; set; } = null!;
}

