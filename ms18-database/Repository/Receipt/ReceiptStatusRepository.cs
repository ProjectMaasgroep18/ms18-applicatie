using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Receipts
{
    public class ReceiptStatusRepository : IReceiptStatusRepository
    {
        const ReceiptStatus UnknownStatus = ReceiptStatus.Onbekend; 

        public bool Exists(long id)
            => Enum.IsDefined(typeof(ReceiptStatus), id);

        public string? GetById(long id)
            => GetModel(id).ToString();

        public ReceiptStatus GetModel(long id)
            => Enum.IsDefined(typeof(ReceiptStatus), id) ? (ReceiptStatus)id : UnknownStatus;

        public ReceiptStatus GetModel(string value)
            => Enum.TryParse(value, true, out ReceiptStatus result) ? result : UnknownStatus;

        public IEnumerable<ReceiptStatus> ListAll(int offset = 0, int limit = 0)
            => Enum.GetValues(typeof(ReceiptStatus)).Cast<ReceiptStatus>().ToList();
    }
}
