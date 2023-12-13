using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptRepository<TReceiptRecord, TReceiptHistory> : IGenericRepository<TReceiptRecord, ReceiptModel, TReceiptHistory>
	{
		
	}
}
