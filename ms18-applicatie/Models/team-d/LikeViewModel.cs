namespace ms18_applicatie.Models.team_d;

public class LikeViewModel
{
    public Guid Id { get; set; }

    public long MemberId { get; set; }

    public Guid PhotoId { get; set; }

    public DateTime LikedOn { get; set; }
}

