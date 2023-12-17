using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Services
{
	public class MemberService : IMemberService
	{
		private readonly IMemberRepository _memberRepository;
		private readonly ITokenStoreRepository _tokenStoreRepository;

		public MemberService(IMemberRepository memberRepository, ITokenStoreRepository tokenStoreRepository)
		{
			_memberRepository = memberRepository;
			_tokenStoreRepository = tokenStoreRepository;
		}

		public bool MemberExists(long id)
		{
			return _memberRepository.Exists(id);
		}

		public MemberModel? GetMember(long id)
		{
			return _memberRepository.GetModel(id);
		}

		public MemberModel? GetMemberByToken(string token)
		{
			var memberId = _tokenStoreRepository.GetMemberIdFromToken(token);
			if (memberId == null)
				return null;
			var currentMember = _memberRepository.GetModel((long)memberId);
			return currentMember;
		}
	}
}
