using System.Text.Json.Serialization;
using Maasgroep.Database.Photos;

namespace Maasgroep.Database.Repository.ViewModel;
public class PhotoViewModel
{
    
    public long? Id { get; set; }
    public long? Receipt { get; set; }
    public string fileExtension { get; set; }
    public string fileName { get; set; }
    public DateTime DateTimeCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateTimeModified { get; set; }
    public string Base64Image { get; set; }
    
    

    [JsonConstructor]
    public PhotoViewModel() { }
    
    public PhotoViewModel(Photo photo)
    {
        Id = photo.Id;
        Receipt = photo.Receipt;
        fileExtension = photo.fileExtension;
        fileName = photo.fileName;
        DateTimeCreated = photo.DateTimeCreated;
        DateTimeModified = photo.DateTimeModified;
        Base64Image = photo.Base64Image;
    }
    
}