using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
	/** Receipt Approval repository interface, connecting to Receipt Approval database records */
	public interface IReceiptApprovalRepository : IWritableRepository<ReceiptApproval, ReceiptApprovalModel>
	{

	}
}
