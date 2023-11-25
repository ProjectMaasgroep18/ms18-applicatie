using System.Text.Json.Serialization;
using Maasgroep.Database.Photos;

namespace Maasgroep.Database.Repository.ViewModel;
public class PhotoViewModel
{
    
    public long Id { get; set; }
    public long ReceiptId { get; set; }
    public string fileExtension { get; set; }
    public string fileName { get; set; }
    public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateTimeModified { get; set; }
    public string Base64Image { get; set; }
    
    

    [JsonConstructor]
    public PhotoViewModel() { }
    
    public static PhotoViewModel FromDatabaseModel(Photo photo)
    {
        return new PhotoViewModel
        {
            Id = photo.Id,
            ReceiptId = photo.ReceiptId,
            fileExtension = photo.fileExtension,
            fileName = photo.fileName,
            DateTimeCreated = photo.DateTimeCreated,
            DateTimeModified = photo.DateTimeModified,
            Base64Image = photo.Base64Image,
            // Map other properties as needed
        };
    }
    
}