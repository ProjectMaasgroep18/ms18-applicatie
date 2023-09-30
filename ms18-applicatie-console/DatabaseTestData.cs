using Maasgroep.Database;

namespace ms18_applicatie_console
{
    internal class DatabaseTestData
    {
        internal DatabaseTestData() { }

        internal void CreateTestDataAll()
        {
            CreateTestDataMember();
            CreateTestDataCostCentre();
            CreateTestDataStore();
            CreateTestDataReceiptStatus();
            CreateTestDataReceipt();
            CreateTestDataReceiptApproval();
        }



        internal void CreateTestDataMember()
        {

            using (var db = new MaasgroepContext())
            {
                var members = new List<MaasgroepMember>()
                {
                    new MaasgroepMember() { Name = "Borgia"}
                };

                db.MaasgroepMember.AddRange(members);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");

                var borgia = db.MaasgroepMember.FirstOrDefault()!;

                members = new List<MaasgroepMember>()
                {
                    new MaasgroepMember() { Name = "da Gama", UserCreated = borgia }
                ,   new MaasgroepMember() { Name = "Albuquerque", UserCreated = borgia }
                };

                borgia.UserCreated = borgia;
                borgia.UserModified = borgia;
                borgia.DateTimeModified = DateTime.UtcNow;

                db.MaasgroepMember.AddRange(members);

                rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        internal void CreateTestDataCostCentre()
        {
            using (var db = new MaasgroepContext())
            {

                var member = db.MaasgroepMember.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var costCentres = new List<CostCentre>()
                {
                    new CostCentre() { Name = "Bestuur Maasgroep", UserCreated = member }
                ,   new CostCentre() { Name = "Penningmeester", UserCreated = member }
                ,   new CostCentre() { Name = "Moeder van Joopie", UserCreated = member }
                };

                db.CostCentre.AddRange(costCentres);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        internal void CreateTestDataStore()
        {
            using (var db = new MaasgroepContext())
            {

                var member = db.MaasgroepMember.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var stores = new List<Store>()
                {
                    new Store() { Name = "Albert Gein", UserCreated = member }
                ,   new Store() { Name = "Supboards Limited", UserCreated = member }
                };

                db.Store.AddRange(stores);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        internal void CreateTestDataReceiptStatus()
        {
            using (var db = new MaasgroepContext())
            {

                var member = db.MaasgroepMember.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var receiptStati = new List<ReceiptStatus>()
                {
                    new ReceiptStatus() { Name = "Ingediend", UserCreated = member }
                ,   new ReceiptStatus() { Name = "Goedgekeurd", UserCreated = member }
                ,   new ReceiptStatus() { Name = "Afgekeurd", UserCreated = member }
                ,   new ReceiptStatus() { Name = "Uitbetaald", UserCreated = member }
                };

                db.ReceiptStatus.AddRange(receiptStati);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        internal void CreateTestDataReceipt()
        {
            using (var db = new MaasgroepContext())
            {
                var member = db.MaasgroepMember.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var costCentre = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var store = db.Store.Where(s => s.Name == "Supboards Limited").FirstOrDefault()!;
                var receiptStatusIngediend = db.ReceiptStatus.Where(rs => rs.Name == "Ingediend").FirstOrDefault()!;
                var receiptStatusGoedgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Goedgekeurd").FirstOrDefault()!;
                var receiptStatusAfgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Afgekeurd").FirstOrDefault()!;

                var receipts = new List<Receipt>()
                {
                    new Receipt()   { UserCreated = member, ReceiptStatus = receiptStatusIngediend
                                    , Store = store, CostCentre = costCentre
                                    }
                ,   new Receipt()   { UserCreated = member, ReceiptStatus = receiptStatusGoedgekeurd
                                    , Store = store, CostCentre = costCentre
                                    }
                ,   new Receipt()   { UserCreated = member, ReceiptStatus = receiptStatusAfgekeurd
                                    , Store = store, CostCentre = costCentre
                                    }
                };

                db.Receipt.AddRange(receipts);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        internal void CreateTestDataReceiptApproval()
        {
            using (var db = new MaasgroepContext())
            {
                var member = db.MaasgroepMember.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var costCentre = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var store = db.Store.Where(s => s.Name == "Supboards Limited").FirstOrDefault()!;
                var receiptStatusIngediend = db.ReceiptStatus.Where(rs => rs.Name == "Ingediend").FirstOrDefault()!;
                var receiptStatusGoedgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Goedgekeurd").FirstOrDefault()!;
                var receiptStatusAfgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Afgekeurd").FirstOrDefault()!;
                var receiptGoedgekeurd = db.Receipt.Where(r => r.ReceiptStatus == receiptStatusGoedgekeurd).FirstOrDefault()!;
                var receiptAfgekeurd = db.Receipt.Where(r => r.ReceiptStatus == receiptStatusAfgekeurd).FirstOrDefault()!;

                var receiptApprovals = new List<ReceiptApproval>()
                {
                    new ReceiptApproval() { Receipt = receiptGoedgekeurd, Note = "Lekker duidelijk met zo'n foto!", UserCreated = member }
                ,   new ReceiptApproval() { Receipt = receiptAfgekeurd, Note = "Dit is niet het soort plug dat we nodig hebben.", UserCreated = member }
                };

                db.ReceiptApproval.AddRange(receiptApprovals);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
    }
}
