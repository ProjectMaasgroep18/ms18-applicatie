using Maasgroep.Database.team_d.Models;

namespace ms18_applicatie.Models.team_d;
public class PhotosInDirectoryResponse
{
    public List<Photo> Photos { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

