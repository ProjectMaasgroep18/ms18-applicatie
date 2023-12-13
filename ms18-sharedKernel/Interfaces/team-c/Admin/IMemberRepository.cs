using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.Interfaces.Admin
{
	public interface IMemberRepository<TRecord> : IDeletableRepository<TRecord, MemberModel>
	{
		
	}
}
