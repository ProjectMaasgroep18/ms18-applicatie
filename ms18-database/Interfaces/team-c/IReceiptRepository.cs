using Maasgroep.Database.Interfaces;

namespace Maasgroep.Database.Receipts
{
    public interface IReceiptRepository
    {
        long AddReceipt(IReceipt receipt);

        bool ModifyReceipt(IReceipt receipt);

        // Nadenken over GetReceipt en model

        void AanmakenTestData();
    }
}
