
namespace Maasgroep.SharedKernel.ViewModels.Admin
{
	public class MemberModel
	{
		public MemberModel() 
		{
			Permissions = new List<PermissionModel>();
		}
		public long Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public IEnumerable<PermissionModel> Permissions { get; set; }

	}
}
