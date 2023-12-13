namespace Maasgroep.Database.team_d.Models;

public class PhotoTag
{
    public Guid PhotoId { get; set; }
    public Photo Photo { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
