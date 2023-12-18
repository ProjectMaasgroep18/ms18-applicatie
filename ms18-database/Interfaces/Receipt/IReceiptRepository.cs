using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
	/** Receipt repository interface, connecting to Receipt database records */
	public interface IReceiptRepository : IEditableRepository<Receipt, ReceiptModel, ReceiptData, ReceiptHistory>
	{
		IEnumerable<ReceiptModel> ListByCostCentre(long costCentreId, int offset = default, int limit = default, bool includeDeleted = default);
		IEnumerable<ReceiptModel> ListPayable(int offset = default, int limit = default, bool includeDeleted = default);
	}
}
