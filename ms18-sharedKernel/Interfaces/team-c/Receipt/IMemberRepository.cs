using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.Interfaces.Members
{
	public interface IMemberRepository
	{
		MemberModel GetMember(long id);
		MemberModel GetMemberByEmail(string email);

        bool MemberExists(long id);
	}
}
