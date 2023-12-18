using Maasgroep.SharedKernel.ViewModels.Admin;

namespace ms18_applicatie.Services
{
	public interface IMemberService
	{
		MemberModel GetMember(long id);
		bool MemberExists(long id);
	}
}