using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.DataModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
    /** Receipt Photo repository interface, connecting to Receipt Photo database records */
    public interface IReceiptPhotoRepository : IDeletableRepository<ReceiptPhoto, ReceiptPhotoModel, ReceiptPhotoData>
    {
        IEnumerable<ReceiptPhotoModel> ListByReceipt(long receiptId, int offset = default, int limit = default, bool includeDeleted = default);
    }
}
