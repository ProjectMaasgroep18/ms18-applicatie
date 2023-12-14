using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptStatusRepository : IReceiptStatusRepository
    {
        public bool Exists(long id)
            => Enum.IsDefined(typeof(ReceiptStatus), id);

        public string? GetById(long id)
            => GetModel(id).ToString();

        public ReceiptStatus GetModel(long id)
            => Enum.IsDefined(typeof(ReceiptStatus), id) ? (ReceiptStatus)id : ReceiptStatus.Onbekend;

        public ReceiptStatus GetModel(string value)
        {
            throw new NotImplementedException();
        }

        public string? GetRecord(ReceiptStatus model, string? existingRecord = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReceiptStatus> ListAll(int offset = 0, int limit = 0)
        {
            throw new NotImplementedException();
        }
    }
}
