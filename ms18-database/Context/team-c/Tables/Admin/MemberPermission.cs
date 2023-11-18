
namespace Maasgroep.Database.Members
{
    public record MemberPermission
    {
        public long MemberId { get; set; }
        public long PermissionId { get; set; }


        // Generic
        public long MemberCreatedId { get; set; }
        public long? MemberModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


        // EF admin properties
        public Member Member { get; set; }
        public Permission Permission { get; set; }


        // EF generic properties
        public Member MemberCreated { get; set; }
        public Member? MemberModified { get; set; }

    }
}
