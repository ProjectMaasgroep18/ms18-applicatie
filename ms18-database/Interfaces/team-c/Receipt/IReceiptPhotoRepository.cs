using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
	/** Receipt Photo repository interface, connecting to Receipt Photo database records */
	public interface IReceiptPhotoRepository : IDeletableRepository<ReceiptPhoto, ReceiptPhotoModel>
	{
		IEnumerable<ReceiptPhotoModel> ListByReceipt(long receiptId, int offset = default, int limit = default, bool includeDeleted = default);
	}
}
