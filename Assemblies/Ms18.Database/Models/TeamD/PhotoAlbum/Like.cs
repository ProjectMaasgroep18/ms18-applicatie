using Ms18.Database.Models.TeamC.Admin;

namespace Ms18.Database.Models.TeamD.PhotoAlbum;
public record Like
{
    public Guid Id { get; set; }

    public long MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public Guid PhotoId { get; set; }
    public Photo Photo { get; set; } = null!;

    public DateTime LikedOn { get; set; }
}
