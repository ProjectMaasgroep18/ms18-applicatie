using Maasgroep.Database.Admin;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Interfaces
{
	/** Member repository interface, connecting to Member database records */
	public interface IMemberRepository : IWritableRepository<Member, MemberModel>
	{

	}
}
