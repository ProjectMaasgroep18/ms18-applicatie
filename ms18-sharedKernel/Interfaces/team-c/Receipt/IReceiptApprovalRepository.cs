using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptApprovalRepository<TRecord> : IGenericRepository<TRecord, ReceiptApprovalModel>
	{
		
	}
}
