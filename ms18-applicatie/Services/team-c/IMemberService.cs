using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Services
{
	public interface IMemberService
	{
		MemberModel GetMember(long id);
		bool MemberExists(long id);
	}
}