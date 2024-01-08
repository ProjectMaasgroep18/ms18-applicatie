using Maasgroep.Database.Admin;

namespace Maasgroep.Database.Context.Tables.PhotoAlbum;

public class Photo
{
    public Photo()
    {
    }

    public Photo(long uploaderId, byte[] imageData, string contentType, bool needsApproval, Guid albumLocationId)
    {
        UploaderId = uploaderId;
        UploadDate = DateTime.UtcNow;
        ImageData = imageData;
        ContentType = contentType;
        NeedsApproval = needsApproval;
        AlbumLocationId = albumLocationId;
    }
    public Guid Id { get; set; }

    public long UploaderId { get; set; }
    public Member Uploader { get; set; } = null!;
    public DateTime UploadDate { get; set; }

    public string? Title { get; set; }
    public byte[] ImageData { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public DateTime? TakenOn { get; set; }
    public string? Location { get; set; }
    public bool NeedsApproval { get; set; }

    public Guid AlbumLocationId { get; set; }
    public Album AlbumLocation { get; set; } = null!;

    public IEnumerable<Like> Likes { get; set; } = null!;
}

