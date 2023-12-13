using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptStatusRepository<TRecord> : IReadOnlyRepository<TRecord, ReceiptStatus>
	{
		
	}
}
