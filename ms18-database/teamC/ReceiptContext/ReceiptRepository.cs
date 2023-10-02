
namespace Maasgroep.Database.Receipts
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ReceiptContext _db;
        public ReceiptRepository() 
        { 
            _db = new ReceiptContext();
        }

        public void AddReceipt()
        {
            // Hier logica bouwen
            // ReceiptRepository = zichtbaar voor buitenkant (public)
            // Context = zichtbaar in repository (internal)
        }

        public void ModifyReceipt()
        {
            // Hier logica bouwen
            // ReceiptRepository = zichtbaar voor buitenkant (public)
            // Context = zichtbaar in repository (internal)
        }
    }
}
