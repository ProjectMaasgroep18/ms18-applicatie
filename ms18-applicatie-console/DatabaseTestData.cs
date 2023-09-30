using Maasgroep.Database;

namespace ms18_applicatie_console
{
    internal class DatabaseTestData
    {
        internal DatabaseTestData() { }

        internal void CreateTestDataAll()
        {
            //CreateTestDataMember();
            //CreateTestDataCostCentre();
            //CreateTestDataStore();
            //CreateTestDataReceiptStatus();
            CreateTestDataReceipt();
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

                db.MaasgroepMember.AddRange(members);

                rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        internal void CreateTestDataCostCentre()
        {
            using (var db = new MaasgroepContext())
            {

                var member = db.MaasgroepMember.Where(m => m.Id == 1).FirstOrDefault()!;

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

                var member = db.MaasgroepMember.Where(m => m.Id == 1).FirstOrDefault()!;

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

                var member = db.MaasgroepMember.Where(m => m.Id == 1).FirstOrDefault()!;

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

                var member = db.MaasgroepMember.Where(m => m.Id == 1).FirstOrDefault()!;
                var costCentre = db.CostCentre.Where(cc => cc.Id == 3).FirstOrDefault()!;
                var store = db.Store.Where(s => s.Id == 1).FirstOrDefault()!;
                var receiptStatusIngediend = db.ReceiptStatus.Where(rs => rs.Id == 1).FirstOrDefault()!;
                var receiptStatusGoedgekeurd = db.ReceiptStatus.Where(rs => rs.Id == 2).FirstOrDefault()!;
                var receiptStatusAfgekeurd = db.ReceiptStatus.Where(rs => rs.Id == 3).FirstOrDefault()!;

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
    }
}
