using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Test
{
    public class ReceiptTestFixture
    {
        private const string ConnectionString = @"UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=MaasgroepTest;Pooling=true";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public ReceiptTestFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    InitDatabase();

                    // Members / admin
                    CreateTestDataMember();
                    CreateTestDataPermissions();
                    CreateTestDataMemberPermissions();

                    // Receipt
                    CreateTestDataCostCentre();
                    CreateTestDataReceipt();
                    CreateTestDataReceiptApproval();

                    _databaseInitialized = true;
                }
            }
        }

        public ReceiptContext CreateContext()
            => new ReceiptContext(ConnectionString);

        private void InitDatabase()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.SaveChanges();
            }
        }

        #region Member_Admin
        private void CreateTestDataMember()
        {
            //using (var db = CreateContext())
            //{
            //    var members = new List<Member>()
            //    {
            //        new Member() { Name = "Borgia"}
            //    };

            //    db.Member.AddRange(members);

            //    db.SaveChanges();

            //    var borgia = db.Member.FirstOrDefault()!;

            //    members = new List<Member>()
            //    {
            //        new Member() { Name = "da Gama", MemberCreated = borgia }
            //    ,   new Member() { Name = "Albuquerque", MemberCreated = borgia }
            //    };

            //    borgia.MemberCreated = borgia;
            //    borgia.MemberModified = borgia;
            //    borgia.DateTimeModified = DateTime.UtcNow;

            //    db.Member.AddRange(members);

            //    db.SaveChanges();
            //}
        }
        private void CreateTestDataPermissions()
        {
            //using (var db = CreateContext())
            //{
            //    var borgia = db.Member.FirstOrDefault()!;

            //    var permissions = new List<Permission>()
            //    {
            //        new Permission() { Name = "receipt.approve", MemberCreated = borgia}
            //    ,   new Permission() { Name = "receipt.reject", MemberCreated = borgia}
            //    ,   new Permission() { Name = "receipt.handIn", MemberCreated = borgia}
            //    ,   new Permission() { Name = "receipt.payOut", MemberCreated = borgia}
            //    };

            //    db.Permission.AddRange(permissions);

            //    db.SaveChanges();
            //}
        }
        private void CreateTestDataMemberPermissions()
        {
            //using (var db = CreateContext())
            //{
            //    var borgia = db.Member.FirstOrDefault()!;
            //    var daGama = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;
            //    var alb = db.Member.Where(m => m.Name == "Albuquerque").FirstOrDefault()!;

            //    var approve = db.Permission.Where(p => p.Name == "receipt.approve").FirstOrDefault()!;
            //    var reject = db.Permission.Where(p => p.Name == "receipt.reject").FirstOrDefault()!;
            //    var handIn = db.Permission.Where(p => p.Name == "receipt.handIn").FirstOrDefault()!;

            //    var memberPermissions = new List<MemberPermission>()
            //    {
            //        new MemberPermission() { Member = daGama, Permission = approve, MemberCreated = borgia }
            //    ,   new MemberPermission() { Member = daGama, Permission = reject, MemberCreated = borgia }
            //    ,   new MemberPermission() { Member = alb, Permission = handIn, MemberCreated = borgia }
            //    };

            //    db.MemberPermission.AddRange(memberPermissions);

            //    db.SaveChanges();
            //}
        }
        #endregion

        #region Receipt
        private void CreateTestDataCostCentre()
        {
            //using (var db = CreateContext())
            //{
            //    var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

            //    var costCentres = new List<CostCentre>()
            //    {
            //        new CostCentre() { Name = "Bestuur Maasgroep", MemberCreated = member }
            //    ,   new CostCentre() { Name = "Penningmeester", MemberCreated = member }
            //    ,   new CostCentre() { Name = "Moeder van Joopie", MemberCreated = member }
            //    };

            //    db.CostCentre.AddRange(costCentres);

            //    db.SaveChanges();
            //}
        }
        private void CreateTestDataReceipt()
        {
    //        using (var db = CreateContext())
    //        {
    //            var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
    //            var costCentre1 = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
				//var costCentre2 = db.CostCentre.Where(cc => cc.Name == "Penningmeester").FirstOrDefault()!;

				//var receipts = new List<Receipt>()
    //            {
    //                new Receipt()   { MemberCreated = member, ReceiptStatus = "Ingediend"
    //                                , CostCentre = costCentre1
    //                                }
    //            ,   new Receipt()   { MemberCreated = member, ReceiptStatus = "Goedgekeurd"
    //                                , CostCentre = costCentre2
    //                                }
    //            ,   new Receipt()   { MemberCreated = member, ReceiptStatus = "Afgekeurd"
    //                                , CostCentre = costCentre1
    //                                }
    //            };

    //            db.Receipt.AddRange(receipts);

    //            db.SaveChanges();
    //        }
        }

        private void CreateTestDataReceiptApproval()
        {
            //using (var db = CreateContext())
            //{
            //    var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
            //    var costCentre = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
            //    var receiptGoedgekeurd = db.Receipt.Where(r => r.ReceiptStatus == "Goedgekeurd").FirstOrDefault()!;
            //    var receiptAfgekeurd = db.Receipt.Where(r => r.ReceiptStatus == "Afgekeurd").FirstOrDefault()!;

            //    var receiptApprovals = new List<ReceiptApproval>()
            //    {
            //        new ReceiptApproval() { Receipt = receiptGoedgekeurd, Note = "Lekker duidelijk met zo'n foto!", MemberCreated = member }
            //    ,   new ReceiptApproval() { Receipt = receiptAfgekeurd, Note = "Dit is niet het soort plug dat we nodig hebben.", MemberCreated = member }
            //    };

            //    db.ReceiptApproval.AddRange(receiptApprovals);

            //    db.SaveChanges();
            //}
        }
        #endregion

    }
}
