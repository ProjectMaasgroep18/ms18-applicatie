
namespace Maasgroep.Database
{
    public abstract record GenericRecordHistory
    {
        // History
        public long Id { get; set; } //AlternateKey
        public DateTime RecordCreated { get; set; }

        // Generic
        public virtual long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public long? MemberDeletedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public DateTime? DateTimeDeleted { get; set; }
    }
}
