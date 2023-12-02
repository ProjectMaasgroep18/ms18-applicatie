﻿using System.Text.Json.Serialization;
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
    “CostCentre”: URI,
    “CostCentreId”: URI,
    “CostCentreURI”: URI,
    */
    
    public class ReceiptViewModel
    {
        
        public long? Id { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public decimal? Amount { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public List<string>? ReceiptPhotoURI { get; set; }
        public long? CostCentreId { get; set; }
        public string? CostCentre { get; set; }
        public string? CostCentreURI { get; set; }
        public long? PhotoId { get; set; }

        public ReceiptViewModel(int id, DateTime dateTimeCreated, DateTime dateTimeModified, decimal amount, string note, string status)
        {
            Id = id;
            DateTimeCreated = dateTimeCreated;
            DateTimeModified = dateTimeModified;
            Amount = amount;
            Note = note;
            Status = status;
        }

        public ReceiptViewModel(Receipt dbRec)
        {
            // Create ReceiptViewModel from Database Model

            Id = dbRec.Id;
            DateTimeCreated = dbRec.DateTimeCreated;
            DateTimeModified = dbRec.DateTimeModified;
            Amount = dbRec.Amount;
            Note = dbRec.Note;
            Status = dbRec.ReceiptStatus;
        }
        
        [JsonConstructor]
        public ReceiptViewModel() { }
        
    }
}
