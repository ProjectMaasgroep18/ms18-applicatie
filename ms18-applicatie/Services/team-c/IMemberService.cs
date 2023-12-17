using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Services
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