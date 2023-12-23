using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.Interfaces;

namespace Maasgroep.Services
{
	public class MaasgroepAuthenticationService : IMaasgroepAuthenticationService
	{
		public MemberModel GetCurrentMember(HttpContext httpContext)
		{
			if (httpContext?.Items["CurrentUser"] is MemberModel validModel)
				return validModel;

			throw new Exception("kapot!");
		}
	}
}
