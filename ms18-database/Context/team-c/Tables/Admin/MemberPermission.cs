
namespace Maasgroep.Database.Members
{
    public record MemberPermission : MemberRecordActive
	{
        public long MemberId { get; set; }
        public long PermissionId { get; set; }


        // EF admin properties
        public Member Member { get; set; }
        public Permission Permission { get; set; }
    }
}
