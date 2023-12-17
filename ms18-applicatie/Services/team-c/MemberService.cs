using Maasgroep.SharedKernel.Interfaces.Members;
using Maasgroep.SharedKernel.Interfaces.Token;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace ms18_applicatie.Services
{
	public class MemberService : IMemberService
	{
		private readonly IMemberRepository _memberRepository;
		private readonly ITokenStoreRepository _tokenStoreRepository;

		public MemberModel? CurrentMember { get; private set; }

		public MemberService(IMemberRepository memberRepository, ITokenStoreRepository tokenStoreRepository)
		{
			_memberRepository = memberRepository;
			_tokenStoreRepository = tokenStoreRepository;
		}

		public bool MemberExists(long id)
		{
			return _memberRepository.MemberExists(id);
		}

		public MemberModel? GetMember(long id)
		{
			return _memberRepository.GetMember(id);
		}

		public MemberModel GetMemberByEmail(string email)
		{
			return _memberRepository.GetMemberByEmail(email);
		}

		public MemberModel? GetMemberByToken(string token)
		{
			var memberId = _tokenStoreRepository.GetMemberFromToken(token);
			if (memberId == null)
				return null;
			CurrentMember = _memberRepository.GetMember((long)memberId);
			return CurrentMember;
		}
	}
}
