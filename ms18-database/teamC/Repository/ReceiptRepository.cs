
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
            using (var db = new MaasgroepContext())
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
        private void CreateTestDataReceiptStatus()
        {
            using (var db = new MaasgroepContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var receiptStati = new List<ReceiptStatus>()
                {
                    new ReceiptStatus() { Name = "Ingediend", MemberCreated = member }
                ,   new ReceiptStatus() { Name = "Goedgekeurd", MemberCreated = member }
                ,   new ReceiptStatus() { Name = "Afgekeurd", MemberCreated = member }
                ,   new ReceiptStatus() { Name = "Uitbetaald", MemberCreated = member }
                };

                db.ReceiptStatus.AddRange(receiptStati);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
        private void CreateTestDataReceipt()
        {
            using (var db = new MaasgroepContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var costCentre = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var receiptStatusIngediend = db.ReceiptStatus.Where(rs => rs.Name == "Ingediend").FirstOrDefault()!;
                var receiptStatusGoedgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Goedgekeurd").FirstOrDefault()!;
                var receiptStatusAfgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Afgekeurd").FirstOrDefault()!;

                var receipts = new List<Receipt>()
                {
                    new Receipt()   { MemberCreated = member, ReceiptStatus = receiptStatusIngediend
                                    , CostCentre = costCentre
                                    }
                ,   new Receipt()   { MemberCreated = member, ReceiptStatus = receiptStatusGoedgekeurd
                                    , CostCentre = costCentre
                                    }
                ,   new Receipt()   { MemberCreated = member, ReceiptStatus = receiptStatusAfgekeurd
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
            using (var db = new MaasgroepContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var costCentre = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var receiptStatusIngediend = db.ReceiptStatus.Where(rs => rs.Name == "Ingediend").FirstOrDefault()!;
                var receiptStatusGoedgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Goedgekeurd").FirstOrDefault()!;
                var receiptStatusAfgekeurd = db.ReceiptStatus.Where(rs => rs.Name == "Afgekeurd").FirstOrDefault()!;
                var receiptGoedgekeurd = db.Receipt.Where(r => r.ReceiptStatus == receiptStatusGoedgekeurd).FirstOrDefault()!;
                var receiptAfgekeurd = db.Receipt.Where(r => r.ReceiptStatus == receiptStatusAfgekeurd).FirstOrDefault()!;

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
    }
}
