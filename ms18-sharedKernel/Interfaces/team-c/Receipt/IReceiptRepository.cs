using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Interfaces.Receipts
{
	public interface IReceiptRepository<TRecord, THistory> : IEditableRepository<TRecord, ReceiptModel, THistory>
	{
		IEnumerable<ReceiptModel> ListByCostCentre(long costCentreId, int offset = default, int limit = default, bool includeDeleted = default);
	}
}
