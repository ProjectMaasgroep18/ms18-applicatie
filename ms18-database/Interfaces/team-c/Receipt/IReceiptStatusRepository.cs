using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
	public interface IReceiptStatusRepository : IReadOnlyRepository<string, ReceiptStatus>
	{
		
	}
}
