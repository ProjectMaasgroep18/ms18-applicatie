
namespace Maasgroep.Database.Receipts
{
    internal record ReceiptHistory
    {
        public long Id { get; set; }
        public DateTime RecordCreated { get; set; }
        public long ReceiptId { get; set; }
        public decimal? Amount { get; set; }
        public long? StoreId { get; set; }
        public long? CostCentreId { get; set; }
        public long ReceiptStatusId { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }


        /* IK DENK DAT DIT NIET NODIG IS
         * WE ZETTEN DATA DOOR / OVER, GEEN CONTROLE
        // EF receipt properties
        public CostCentre? CostCentre { get; set; }
        public ReceiptStatus ReceiptStatus { get; set; }
        public Photo? Photo { get; set; }
        public ReceiptApproval? ReceiptApproval { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        */
    }
}
