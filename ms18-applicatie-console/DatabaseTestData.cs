using Maasgroep.Database.Context;
using Maasgroep.Database.Members;
using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Console
{
    internal class DatabaseTestData
    {
        private DbContext _context;

        internal void CreateTestDataAll()
        {
            AanmakenTestDataMembers();
            AanmakenTestDataReceipts();
            AanmakenTestDataOrders();
        }


        private void AanmakenTestDataMembers()
        {
            CreateTestDataMember();
            CreateTestDataPermissions();
            CreateTestDataMemberPermissions();
        }

        private void AanmakenTestDataReceipts()
        {
            CreateTestDataCostCentre();
            CreateTestDataReceipt();
            CreateTestDataReceiptApproval();
        }

        private void AanmakenTestDataOrders()
        {
            CreateTestDataProduct();
            CreateTestDataStock();
            CreateTestDataProductPrice();
            CreateTestDataBill();
            CreateTestDataLine();
        }

        private MaasgroepContext CreateContext()
        {
            return new MaasgroepContext("UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=Maasgroep;Pooling=true");
        }

        #region Member_Admin

        private void CreateTestDataMember()
        {
            using (var db = CreateContext())
            {
                var members = new List<Member>()
                {
                    new Member() { Name = "Borgia" }
                };

                db.Member.AddRange(members);

                db.SaveChanges();

                var borgia = db.Member.FirstOrDefault()!;

                members = new List<Member>()
                {
                    new Member() { Name = "da Gama", MemberCreated = borgia },
                    new Member() { Name = "Albuquerque", MemberCreated = borgia }
                };

                borgia.MemberCreated = borgia;
                borgia.MemberModified = borgia;
                borgia.DateTimeModified = DateTime.UtcNow;

                db.Member.AddRange(members);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataPermissions()
        {
            using (var db = CreateContext())
            {
                var borgia = db.Member.FirstOrDefault()!;

                var permissions = new List<Permission>()
                {
                    new Permission() { Name = "receipt.approve", MemberCreated = borgia },
                    new Permission() { Name = "receipt.reject", MemberCreated = borgia },
                    new Permission() { Name = "receipt.handIn", MemberCreated = borgia },
                    new Permission() { Name = "receipt.payOut", MemberCreated = borgia }
                };

                db.Permission.AddRange(permissions);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataMemberPermissions()
        {
            using (var db = CreateContext())
            {
                var borgia = db.Member.FirstOrDefault()!;
                var daGama = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;
                var alb = db.Member.Where(m => m.Name == "Albuquerque").FirstOrDefault()!;

                var approve = db.Permission.Where(p => p.Name == "receipt.approve").FirstOrDefault()!;
                var reject = db.Permission.Where(p => p.Name == "receipt.reject").FirstOrDefault()!;
                var handIn = db.Permission.Where(p => p.Name == "receipt.handIn").FirstOrDefault()!;

                var memberPermissions = new List<MemberPermission>()
                {
                    new MemberPermission() { Member = daGama, Permission = approve, MemberCreated = borgia },
                    new MemberPermission() { Member = daGama, Permission = reject, MemberCreated = borgia },
                    new MemberPermission() { Member = alb, Permission = handIn, MemberCreated = borgia }
                };

                db.MemberPermission.AddRange(memberPermissions);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        #endregion

        #region Receipt

        private void CreateTestDataCostCentre()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var costCentres = new List<CostCentre>()
                {
                    new CostCentre() { Name = "Bestuur Maasgroep", MemberCreated = member },
                    new CostCentre() { Name = "Penningmeester", MemberCreated = member },
                    new CostCentre() { Name = "Moeder van Joopie", MemberCreated = member }
                };

                db.CostCentre.AddRange(costCentres);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataReceipt()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var costCentre1 = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
                var costCentre2 = db.CostCentre.Where(cc => cc.Name == "Penningmeester").FirstOrDefault()!;

                var receipts = new List<Receipt>()
                {
                    new Receipt()
                    {
                        MemberCreated = member, ReceiptStatus = "Ingediend", CostCentre = costCentre1
                    },
                    new Receipt()
                    {
                        MemberCreated = member, ReceiptStatus = "Goedgekeurd", CostCentre = costCentre2
                    },
                    new Receipt()
                    {
                        MemberCreated = member, ReceiptStatus = "Afgekeurd", CostCentre = costCentre1
                    }
                };

                db.Receipt.AddRange(receipts);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
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
                    new ReceiptApproval()
                    {
                        Receipt = receiptGoedgekeurd, Note = "Lekker duidelijk met zo'n foto!", MemberCreated = member
                    },
                    new ReceiptApproval()
                    {
                        Receipt = receiptAfgekeurd, Note = "Dit is niet het soort plug dat we nodig hebben.",
                        MemberCreated = member
                    }
                };

                db.ReceiptApproval.AddRange(receiptApprovals);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        #endregion

        #region Order

        private void CreateTestDataProduct()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Duifis Scharrelnootjes", MemberCreated = member, Price = 3.45, Color = "#FF0000",
                        Icon = "fas fa-bowl"
                    },
                    new Product() { Name = "Vorta Cola", MemberCreated = member, Price = 3.45, Color = "#FFF000",
                        Icon = "fas fa-bottle" },
                    new Product() { Name = "Jasmijn Thee", MemberCreated = member, Price = 3.45, Color = "#FF00F0",
                        Icon = "fas fa-cup" }
                };
                db.Product.AddRange(products);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataStock()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
                var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

                var stockToAdd = new List<Stock>()
                {
                    new Stock() { MemberCreated = member, Product = product1, Quantity = 5 },
                    new Stock() { MemberCreated = member, Product = product2, Quantity = 10 }
                };

                db.Stock.AddRange(stockToAdd);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataProductPrice()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
                var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

                var priceToAdd = new List<ProductPrice>()
                {
                    new ProductPrice() { MemberCreated = member, Product = product1, Price = 5.5m },
                    new ProductPrice() { MemberCreated = member, Product = product2, Price = 2.5m }
                };

                db.ProductPrice.AddRange(priceToAdd);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataBill()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var member1 = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var member2 = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;

                var billToAdd = new List<Bill>()
                {
                    new Bill() { MemberCreated = member, IsGuest = false, Member = member1 },
                    new Bill() { MemberCreated = member, IsGuest = false, Member = member2 },
                    new Bill()
                    {
                        MemberCreated = member, IsGuest = true, Name = "Neefje van Donald",
                        Note = "Meteen betaald in Duckaten"
                    }
                };

                db.Bills.AddRange(billToAdd);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataLine()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var member1 = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
                var member2 = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;

                var bill1 = db.Bills.Where(b => b.MemberId == member1.Id).FirstOrDefault()!;
                var bill2 = db.Bills.Where(b => b.MemberId == member2.Id).FirstOrDefault()!;
                var bill3 = db.Bills.Where(b => b.IsGuest).FirstOrDefault()!;

                var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
                var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

                var linesToAdd = new List<Line>()
                {
                    new Line() { Bill = bill1, Product = product1, Quantity = 1, MemberCreated = member },
                    new Line() { Bill = bill2, Product = product2, Quantity = 2, MemberCreated = member },
                    new Line() { Bill = bill3, Product = product1, Quantity = 3, MemberCreated = member },
                    new Line() { Bill = bill3, Product = product2, Quantity = 4, MemberCreated = member }
                };

                db.OrderLines.AddRange(linesToAdd);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        #endregion
    }
}