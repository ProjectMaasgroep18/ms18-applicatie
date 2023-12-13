using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptRepository<TReceiptRecord, TReceiptHistory> : IEditableRepository<TReceiptRecord, ReceiptModel, TReceiptHistory>
	{
		
	}
}
