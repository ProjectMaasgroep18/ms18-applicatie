
namespace Maasgroep.Database.Order
{
	public record OrderRecordActive : GenericRecordActive
	{
		// EF generic properties
		public Member MemberCreated { get; set; }
		public Member? MemberModified { get; set; }
		public Member? MemberDeleted { get; set; }

	}
}
