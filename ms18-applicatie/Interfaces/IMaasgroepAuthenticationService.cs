using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Interfaces
{
	public interface IMaasgroepAuthenticationService
	{
		MemberModel? GetCurrentMember(HttpContext httpContext);
	}
}
