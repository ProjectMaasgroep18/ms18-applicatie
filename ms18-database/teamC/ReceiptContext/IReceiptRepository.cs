using Maasgroep.Database.Members;

namespace Maasgroep.Database.Receipts
{
    public interface IReceiptRepository
    {
        void AddReceipt();

        void ModifyReceipt();

        // Nadenken over GetReceipt en model

        void AanmakenTestData();
    }
}
