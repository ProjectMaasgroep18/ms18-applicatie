using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.Database.Receipts
{

    public class ReceiptStatusRepository : IReceiptStatusRepository<string>
    {
        public string? GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ReceiptStatus GetModel(long id)
        {
            throw new NotImplementedException();
        }

        public ReceiptStatus GetModel(string record)
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
