using System.Text.Json.Serialization;
using Ms18.Database.Models.TeamC.Receipt;

namespace Ms18.Database.Repository.TeamC.ViewModel
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
