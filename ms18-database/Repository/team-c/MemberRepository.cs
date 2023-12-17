using Maasgroep.SharedKernel.Interfaces.Members;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Members
{
    public class MemberRepository : IMemberRepository
    {
		private readonly MaasgroepContext _memberContext;

		public MemberRepository(MaasgroepContext memberContext)
        {
			_memberContext = memberContext;
		}

		public long CreateMember(MemberModelCreateDb member)
		{
			return 1;
		}

		public MemberModel GetMember(long id)
		{
			var result = new MemberModel();
			var member = _memberContext.Member.Where(m => m.Id == id).FirstOrDefault();

			if (member != null) 
			{
				result.Name = member.Name;
				result.Id = member.Id;
			}
			return result;
		}

		public MemberModel GetMemberByEmail(string email)
		{
			var result = new MemberModel();
			var member = _memberContext.Member.Where(m => m.Email == email).FirstOrDefault();

			if (member != null)
			{
				result.Name = member.Name;
				result.Id = member.Id;
				result.Email = member.Email;
				result.Password = member.Password;
				result.Permissions = (IEnumerable<PermissionModel>)member.Permissions;
			}

			return result;
		}

		public bool MemberExists(long id)
		{
			return _memberContext.Member.Where(m => m.Id == id).FirstOrDefault() == null ? false : true;
		}
	}
}
