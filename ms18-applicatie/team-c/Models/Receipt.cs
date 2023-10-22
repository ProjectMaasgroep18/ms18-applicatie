using System.Text.Json.Serialization;
using ms18_applicatie.Models;

namespace ms18_applicatie.Models
{
    // Waarom niet gewoon zo (in plaats van een speciale "ReceiptStatus[es]" database tabel)?
    public enum ReceiptStatusSUGGESTIE
    {
        Incomplete,
        Submitted,
        Approved,
        Rejected,
    }

    [Serializable]
    public record Receipt
    {
        public long Id { get; set; }
        public decimal? Amount { get; set; }

        [JsonIgnore] public long? StoreId { get; set; }

        [JsonIgnore] public long? CostCentreId { get; set; }

        [JsonIgnore] public long ReceiptStatusId { get; set; }

        [JsonIgnore] public string? Location { get; set; }

        public string? Note { get; set; }


        // Generic
        [JsonIgnore] public long UserCreatedId { get; set; }
        [JsonIgnore] public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


        // EF receipt properties
        [JsonIgnore] public Store? Store { get; set; }
        [JsonIgnore] public CostCentre? CostCentre { get; set; }
        [JsonIgnore] public ReceiptStatus ReceiptStatus { get; set; }
        [JsonIgnore] public Photo? Photo { get; set; }
        [JsonIgnore] public ReceiptApproval? ReceiptApproval { get; set; }


        // EF generic properties
        [JsonIgnore] public Member UserCreated { get; set; }
        [JsonIgnore] public Member? UserModified { get; set; }

        // API Properties
        public string CostCentreURI => CostCentre == null ? null : "/api/CostCentre/" + CostCentre.Id;
        public string ReceiptPhotoURI => Photo == null ? null : "/api/ReceiptPhoto/" + Photo.Id;
    }
}