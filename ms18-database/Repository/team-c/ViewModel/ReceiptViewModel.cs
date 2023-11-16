using System.Text.Json.Serialization;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Repository.ViewModel
{
    
    /*
    “id”: Number,
    “dateTimeCreated”: DateTime,
    “dateTimeModified”: DateTime,
    “amount”: Number,
    “note”: String
    “ReceiptPhotoURI”: URI[],
    “CostCentreURI”: URI
    */
    
    public class ReceiptViewModel
    {
        
        public long? ID { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public decimal? Amount { get; set; }
        public string? Note { get; set; }
        public long StatusId { get; set; }
        public string? Status { get; set; }
        public List<string> ReceiptPhotoURI { get; set; }
        public long? CostCentreId;
        public string CostCentreURI { get; set; }

        public ReceiptViewModel(Receipt dbRec)
        {
            // Create ReceiptViewModel from Database Model

            ID = dbRec.Id;
            DateTimeCreated = dbRec.DateTimeCreated;
            DateTimeModified = dbRec.DateTimeModified;
            Amount = dbRec.Amount;
            Note = dbRec.Note;
            StatusId = dbRec.ReceiptStatusId;
            CostCentreId = dbRec.CostCentreId;
        }
        
        [JsonConstructor]
        public ReceiptViewModel() { }
        
    }
}
