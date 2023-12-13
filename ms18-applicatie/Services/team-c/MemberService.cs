using Maasgroep.SharedKernel.Interfaces.Members;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Services
{
	public class MemberService : IMemberService
	{
		private readonly IMemberRepository _memberRepository;
		public MemberService(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public bool MemberExists(long id)
		{
			return _memberRepository.MemberExists(id);
		}

		public MemberModel GetMember(long id)
		{
			return _memberRepository.GetMember(id);
		}
	}
}
