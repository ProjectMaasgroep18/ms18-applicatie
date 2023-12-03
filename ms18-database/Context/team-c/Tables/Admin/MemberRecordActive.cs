
namespace Maasgroep.Database.Members
{
	public record MemberRecordActive : GenericRecordActive
	{
		// EF generic properties
		public Member MemberCreated { get; set; }
		public Member? MemberModified { get; set; }
		public Member? MemberDeleted { get; set; }
	}
}
