using Maasgroep.SharedKernel.ViewModels.Admin;

namespace ms18_applicatie.Services
{
	public interface IMemberService
	{
		MemberModel? CurrentMember { get; }
		MemberModel? GetMember(long id);
		MemberModel? GetMemberByEmail(string email);
		MemberModel? GetMemberByToken(string token);
        bool MemberExists(long id);
	}
}