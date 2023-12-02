
namespace Maasgroep.SharedKernel.ViewModels.Admin
{
	public class Member
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<Permission> Permissions { get; set; }

	}
}
