using System.Text.Json.Serialization;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Repository.ViewModel
{
    public class ReceiptApprovalViewModel
    {
        
        public long? ReceiptId { get; set; }
        public string? Note { get; set; }

        public ReceiptApprovalViewModel(ReceiptApproval dbRec)
        {
            ReceiptId = dbRec.ReceiptId;
            Note = dbRec.Note;
        }
        
        [JsonConstructor]
        public ReceiptApprovalViewModel() { }
        
    }
}
