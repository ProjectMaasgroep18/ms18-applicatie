using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptApprovalRepository<TReceiptApprovalRecord> : IGenericRepository<TReceiptApprovalRecord, ReceiptApprovalModel>
	{
		
	}
}
