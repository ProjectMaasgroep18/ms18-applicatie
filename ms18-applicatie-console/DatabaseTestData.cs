using Maasgroep.Database;

namespace ms18_applicatie_console
{
    internal class DatabaseTestData
    {
        internal DatabaseTestData() { }

        internal void CreateTestDataAll()
        {
            CreateTestDataMember();
            CreateTestDataPermissions();
            CreateTestDataMemberPermissions();
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
                var members = new List<Member>()
                {
                    new Member() { Name = "Borgia"}
                };

                db.Member.AddRange(members);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");

                var borgia = db.Member.FirstOrDefault()!;

                members = new List<Member>()
                {
                    new Member() { Name = "da Gama", UserCreated = borgia }
                ,   new Member() { Name = "Albuquerque", UserCreated = borgia }
                };

                borgia.UserCreated = borgia;
                borgia.UserModified = borgia;
                borgia.DateTimeModified = DateTime.UtcNow;

                db.Member.AddRange(members);

                rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        internal void CreateTestDataPermissions()
        {
            using (var db = new MaasgroepContext())
            {
                var borgia = db.Member.FirstOrDefault()!;

                var permissions = new List<Permission>()
                {
                    new Permission() { Name = "receipt.approve", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.reject", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.handIn", UserCreated = borgia}
                ,   new Permission() { Name = "receipt.payOut", UserCreated = borgia}
                };

                db.Permission.AddRange(permissions);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        internal void CreateTestDataMemberPermissions()
        {
            using (var db = new MaasgroepContext())
            {
                var borgia = db.Member.FirstOrDefault()!;
                var daGama = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;
                var alb = db.Member.Where(m => m.Name == "Albuquerque").FirstOrDefault()!;

                var approve = db.Permission.Where(p => p.Name == "receipt.approve").FirstOrDefault()!;
                var reject = db.Permission.Where(p => p.Name == "receipt.reject").FirstOrDefault()!;
                var handIn = db.Permission.Where(p => p.Name == "receipt.handIn").FirstOrDefault()!;

                var memberPermissions = new List<MemberPermission>()
                {
                    new MemberPermission() { Member = daGama, Permission = approve, UserCreated = borgia }
                ,   new MemberPermission() { Member = daGama, Permission = reject, UserCreated = borgia }
                ,   new MemberPermission() { Member = alb, Permission = handIn, UserCreated = borgia }
                };

                db.MemberPermission.AddRange(memberPermissions);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }



        internal void CreateTestDataCostCentre()
        {
            using (var db = new MaasgroepContext())
            {

                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

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

                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

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

                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

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
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
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
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
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
