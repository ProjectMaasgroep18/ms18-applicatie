using Maasgroep.SharedKernel.Interfaces.Members;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Members
{
    public class MemberRepository : IMemberRepository
    {
		private readonly MemberContext _memberContext;

		public MemberRepository(MemberContext memberContext)
        {
			_memberContext = memberContext;
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

		public bool MemberExists(long id)
		{
			return _memberContext.Member.Where(m => m.Id == id).FirstOrDefault() == null ? false : true;
		}
	}
}
