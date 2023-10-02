using Maasgroep.Database.Members;

namespace Maasgroep.Database.Receipts
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ReceiptContext _db;
        private Member _member;
        public ReceiptRepository() 
        { 
            _db = new ReceiptContext();
        }
        public void AanmakenTestData()
        {
            
            _member = NogTeWijzigen();
            CreateTestDataCostCentre();
            CreateTestDataStore();
            CreateTestDataReceiptStatus();
            CreateTestDataReceipt();
            CreateTestDataReceiptApproval();
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

        private void CreateTestDataCostCentre()
        {
            using (_db)
            {

                var costCentres = new List<CostCentre>()
                {
                    new CostCentre() { Name = "Bestuur Maasgroep", UserCreated = _member }
                ,   new CostCentre() { Name = "Penningmeester", UserCreated = _member }
                ,   new CostCentre() { Name = "Moeder van Joopie", UserCreated = _member }
                };

                _db.CostCentre.AddRange(costCentres);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        private void CreateTestDataStore()
        {
            using (_db)
            {
                var stores = new List<Store>()
                {
                    new Store() { Name = "Albert Gein", UserCreated = _member }
                ,   new Store() { Name = "Supboards Limited", UserCreated = _member }
                };

                _db.Store.AddRange(stores);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        private void CreateTestDataReceiptStatus()
        {
            using (_db)
            {
                var receiptStati = new List<ReceiptStatus>()
                {
                    new ReceiptStatus() { Name = "Ingediend", UserCreated = _member }
                ,   new ReceiptStatus() { Name = "Goedgekeurd", UserCreated = _member }
                ,   new ReceiptStatus() { Name = "Afgekeurd", UserCreated = _member }
                ,   new ReceiptStatus() { Name = "Uitbetaald", UserCreated = _member }
                };

                _db.ReceiptStatus.AddRange(receiptStati);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        private void CreateTestDataReceipt()
        {
            using (_db)
            {
                var costCentre = _db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var store = _db.Store.Where(s => s.Name == "Supboards Limited").FirstOrDefault()!;
                var receiptStatusIngediend = _db.ReceiptStatus.Where(rs => rs.Name == "Ingediend").FirstOrDefault()!;
                var receiptStatusGoedgekeurd = _db.ReceiptStatus.Where(rs => rs.Name == "Goedgekeurd").FirstOrDefault()!;
                var receiptStatusAfgekeurd = _db.ReceiptStatus.Where(rs => rs.Name == "Afgekeurd").FirstOrDefault()!;

                var receipts = new List<Receipt>()
                {
                    new Receipt()   { UserCreated = _member, ReceiptStatus = receiptStatusIngediend
                                    , Store = store, CostCentre = costCentre
                                    }
                ,   new Receipt()   { UserCreated = _member, ReceiptStatus = receiptStatusGoedgekeurd
                                    , Store = store, CostCentre = costCentre
                                    }
                ,   new Receipt()   { UserCreated = _member, ReceiptStatus = receiptStatusAfgekeurd
                                    , Store = store, CostCentre = costCentre
                                    }
                };

                _db.Receipt.AddRange(receipts);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        private void CreateTestDataReceiptApproval()
        {
            using (_db)
            {
                var costCentre = _db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var store = _db.Store.Where(s => s.Name == "Supboards Limited").FirstOrDefault()!;
                var receiptStatusIngediend = _db.ReceiptStatus.Where(rs => rs.Name == "Ingediend").FirstOrDefault()!;
                var receiptStatusGoedgekeurd = _db.ReceiptStatus.Where(rs => rs.Name == "Goedgekeurd").FirstOrDefault()!;
                var receiptStatusAfgekeurd = _db.ReceiptStatus.Where(rs => rs.Name == "Afgekeurd").FirstOrDefault()!;
                var receiptGoedgekeurd = _db.Receipt.Where(r => r.ReceiptStatus == receiptStatusGoedgekeurd).FirstOrDefault()!;
                var receiptAfgekeurd = _db.Receipt.Where(r => r.ReceiptStatus == receiptStatusAfgekeurd).FirstOrDefault()!;

                var receiptApprovals = new List<ReceiptApproval>()
                {
                    new ReceiptApproval() { Receipt = receiptGoedgekeurd, Note = "Lekker duidelijk met zo'n foto!", UserCreated = _member }
                ,   new ReceiptApproval() { Receipt = receiptAfgekeurd, Note = "Dit is niet het soort plug dat we nodig hebben.", UserCreated = _member }
                };

                _db.ReceiptApproval.AddRange(receiptApprovals);

                var rows = _db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        private Member NogTeWijzigen()
        {
            Member result = null;

            using (var db = new MemberContext())
            {
                result = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
            }

            return result;
        }
    }
}
