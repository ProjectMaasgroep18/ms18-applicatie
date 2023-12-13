namespace ms18_applicatie.Models.team_d
{
    public class PhotoUploadModel
    {
        public string Title { get; set; } = null!;
        public string ImageBase64 { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public Guid FolderLocationId { get; set; }
        public List<Guid> TagIds { get; set; } = null!;
    }

}
