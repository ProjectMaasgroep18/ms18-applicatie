using Maasgroep.Database.Members;

namespace Maasgroep.Database.team_d.Models;

public record Like
{
    public Guid Id { get; set; }

    public long MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public Guid PhotoId { get; set; }
    public Photo Photo { get; set; } = null!;

    public DateTime LikedOn { get; set; }
}
