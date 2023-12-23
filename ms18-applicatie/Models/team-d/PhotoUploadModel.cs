﻿namespace Maasgroep.Models.team_d
{
    public class PhotoUploadModel
    {
        public string? Title { get; set; }
        public string ImageBase64 { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public Guid AlbumId { get; set; }
        public string? Location { get; set; }
        public DateTime? TakenOn { get; set; }
    }

}
