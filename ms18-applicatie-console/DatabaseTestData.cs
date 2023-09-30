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
                    new MaasgroepMember() { Name = "da Gama"}
                ,   new MaasgroepMember() { Name = "Borgia"}
                ,   new MaasgroepMember() { Name = "Albuquerque"}
                };

                db.MaasgroepMember.AddRange(members);

                var rows = db.SaveChanges();
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
                    new CostCentre() { Name = "Bestuur Maasgroep", UserCreatedInstance = member }
                ,   new CostCentre() { Name = "Penningmeester", UserCreatedInstance = member }
                ,   new CostCentre() { Name = "Moeder van Joopie", UserCreatedInstance = member }
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
                    new Store() { Name = "Albert Gein", UserCreatedInstance = member }
                ,   new Store() { Name = "Supboards Limited", UserCreatedInstance = member }
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
                    new ReceiptStatus() { Name = "Ingediend", UserCreatedInstance = member }
                ,   new ReceiptStatus() { Name = "Goedgekeurd", UserCreatedInstance = member }
                ,   new ReceiptStatus() { Name = "Afgekeurd", UserCreatedInstance = member }
                ,   new ReceiptStatus() { Name = "Uitbetaald", UserCreatedInstance = member }
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
                    new Receipt()   { UserCreatedInstance = member, ReceiptStatusInstance = receiptStatusIngediend
                                    , StoreInstance = store, CostCentreInstance = costCentre
                                    }
                ,   new Receipt()   { UserCreatedInstance = member, ReceiptStatusInstance = receiptStatusGoedgekeurd
                                    , StoreInstance = store, CostCentreInstance = costCentre
                                    }
                ,   new Receipt()   { UserCreatedInstance = member, ReceiptStatusInstance = receiptStatusAfgekeurd
                                    , StoreInstance = store, CostCentreInstance = costCentre
                                    }
                };

                db.Receipt.AddRange(receipts);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
    }
}
