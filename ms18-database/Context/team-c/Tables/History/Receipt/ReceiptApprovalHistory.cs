
namespace Maasgroep.Database.Receipts
{
    public record ReceiptApprovalHistory
    {
        public long Id { get; set; }
        public DateTime RecordCreated { get; set; }
        public long ReceiptId { get; set; }
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
        public Receipt Receipt { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }
        */
    }
}
