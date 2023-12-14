using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
	/** Receipt repository interface, connecting to Receipt database records */
	public interface IReceiptRepository : IEditableRepository<Receipt, ReceiptModel, ReceiptHistory>
	{
		IEnumerable<ReceiptModel> ListByCostCentre(long costCentreId, int offset = default, int limit = default, bool includeDeleted = default);
	}
}
