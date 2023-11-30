using Maasgroep.Database.Interfaces;

namespace Maasgroep.Database.Receipts
{
    public class ReceiptRepository : IReceiptRepository
    {
        public ReceiptRepository() 
        { 
        }
        public void AanmakenTestData()
        {
            CreateTestDataCostCentre();
            CreateTestDataReceipt();
            CreateTestDataReceiptApproval();
        }

        public long AddReceipt(IReceipt receipt)
        {
            // Hier logica bouwen
            // ReceiptRepository = zichtbaar voor buitenkant (public)
            // Context = zichtbaar in repository (internal)

            return -1;
        }

        public bool ModifyReceipt(IReceipt receipt)
        {
            // Hier logica bouwen
            // ReceiptRepository = zichtbaar voor buitenkant (public)
            // Context = zichtbaar in repository (internal)

            return true;
        }

        private void CreateTestDataCostCentre()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var costCentres = new List<CostCentre>()
                {
                    new CostCentre() { Name = "Bestuur Maasgroep", MemberCreated = member }
                ,   new CostCentre() { Name = "Penningmeester", MemberCreated = member }
                ,   new CostCentre() { Name = "Moeder van Joopie", MemberCreated = member }
                };

                db.CostCentre.AddRange(costCentres);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataReceipt()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var costCentre = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;

                var receipts = new List<Receipt>()
                {
                    new Receipt()   { MemberCreated = member, ReceiptStatus = "Ingediend"
                                    , CostCentre = costCentre
                                    }
                ,   new Receipt()   { MemberCreated = member, ReceiptStatus = "Goedgekeurd"
                                    , CostCentre = costCentre
                                    }
                ,   new Receipt()   { MemberCreated = member, ReceiptStatus = "Afgekeurd"
                                    , CostCentre = costCentre
                                    }
                };

                db.Receipt.AddRange(receipts);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        private void CreateTestDataReceiptApproval()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var costCentre = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var receiptGoedgekeurd = db.Receipt.Where(r => r.ReceiptStatus == "Goedgekeurd").FirstOrDefault()!;
                var receiptAfgekeurd = db.Receipt.Where(r => r.ReceiptStatus == "Afgekeurd").FirstOrDefault()!;

                var receiptApprovals = new List<ReceiptApproval>()
                {
                    new ReceiptApproval() { Receipt = receiptGoedgekeurd, Note = "Lekker duidelijk met zo'n foto!", MemberCreated = member }
                ,   new ReceiptApproval() { Receipt = receiptAfgekeurd, Note = "Dit is niet het soort plug dat we nodig hebben.", MemberCreated = member }
                };

                db.ReceiptApproval.AddRange(receiptApprovals);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private MaasgroepContext CreateContext()
        {
            return new MaasgroepContext();
        }
    }
}
