using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.team_c.Repository.ViewModel
{
    
    /*
    id”: Number,
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
        public ReceiptStatusViewModel? ReceiptStatus { get; set; } 

        public ReceiptViewModel(int id, DateTime dateTimeCreated, DateTime dateTimeModified, decimal amount, string note)
        {
            ID = id;
            DateTimeCreated = dateTimeCreated;
            DateTimeModified = dateTimeModified;
            Amount = amount;
            Note = note;
        }

        public ReceiptViewModel(Receipt dbRec)
        {
            ID = dbRec.Id;
            DateTimeCreated = dbRec.DateTimeCreated;
            DateTimeModified = dbRec.DateTimeModified;
            Amount = dbRec.Amount;
            Note = dbRec.Note;
            ReceiptStatus = dbRec.ReceiptStatus == null ? null : new ReceiptStatusViewModel(dbRec.ReceiptStatus); // Deze geeft dus altijd null
        }
        
        [JsonConstructor]
        public ReceiptViewModel() { }
        
    }
}
