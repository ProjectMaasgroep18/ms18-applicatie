using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Services
{
	public interface IMemberService
	{
		MemberModel? GetMember(long id);
		MemberModel? GetMemberByToken(string token);
        bool MemberExists(long id);
	}
}