using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Interfaces
{
    /** Receipt Status repository, connecting to ReceiptStatus enumerable */
    public interface IReceiptStatusRepository : IReadableRepository<string, ReceiptStatus>
    {
        
    }
}
