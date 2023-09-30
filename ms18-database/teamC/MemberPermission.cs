
namespace Maasgroep.Database
{
    public record MemberPermission
    {
        public long MemberId { get; set; }
        public long PermissionId { get; set; }


        // Generic
        public long UserCreatedId { get; set; }
        public long? UserModifiedId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeModified { get; set; }


        // EF admin properties
        public Member Member { get; set; }
        public Permission Permission { get; set; }


        // EF generic properties
        public Member UserCreated { get; set; }
        public Member? UserModified { get; set; }

    }
}
