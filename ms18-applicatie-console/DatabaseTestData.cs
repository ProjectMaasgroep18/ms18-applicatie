using Maasgroep.Database;
using Maasgroep.Database.Admin;
using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Console
{
    internal class DatabaseTestData
    {
        protected Dictionary<string, Member> Members = new();

        internal void CreateTestData()
        {
            try {
                // ADMIN data
                CreateTestDataMember();
            } catch (Microsoft.EntityFrameworkCore.DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
                using (var db = CreateContext())
                {
                    //Zorg dat we alle members hebben
                    Members = db.Member.ToDictionary(m => m.Name, m => m);
                }
            }
            try {
                // PERMISSIONS data [naar admin als t werkt]
                CreateTestDataPermissions();
            } catch (Microsoft.EntityFrameworkCore.DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }
            try {
                // RECEIPT data   
                CreateTestDataCostCentre();
            } catch (Microsoft.EntityFrameworkCore.DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }
            try {
                // PRODUCT data
                CreateTestDataProduct();
                CreateTestDataStock();
                CreateTestDataBill();
                CreateTestDataLine();
            } catch (Microsoft.EntityFrameworkCore.DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }    
        }

        private MaasgroepContext CreateContext()
        {
            return new MaasgroepContext();
        }

        #region Member_Admin

        private void CreateTestDataMember()
        {
            System.Console.WriteLine("CREATE MEMBERS");

            using (var db = CreateContext())
            {
                var admin = Members.ContainsKey("Admin") ? Members["Admin"] : new Member() { Name = "Admin", Email = "admin@example.com", Password = "123456" };
                Members = new Dictionary<string, Member>
                {
                    ["Admin"] = admin,
                 
                    // Team A
                    ["Gast"] = new Member() { Name = "Gast", MemberCreated = admin, Email = "gast@example.com", Password = "123456" },
                    ["Product"] = new Member() { Name = "Product", MemberCreated = admin, Email = "product@example.com", Password = "123456" },

                    // Team C
                    ["Goedkeur"] = new Member() { Name = "Goedkeur", MemberCreated = admin, Email = "goedkeur@example.com", Password = "123456" },
                    ["Betaal"] = new Member() { Name = "Betaal", MemberCreated = admin, Email = "betaal@example.com", Password = "123456" },
                };

                db.Member.AddRange(Members.Select(m => m.Value));

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of members: {rows}");
            }
        }

        private void CreateTestDataPermissions()
        {
            System.Console.WriteLine("CREATE PERMISSIONS");

            using (var db = CreateContext())
            {
                // Toegang tot het order gedeelte: producten zien; eigen bestelling inzien, plaatsen
                var order = new Permission() { Name = "order.submit", MemberCreatedId = 1 };

                // Mag producten toevoegen, wijzigen
                var orderProduct = new Permission() { Name = "order.product", MemberCreatedId = 1 };

                // Toegang tot het "receipt" gedeelte: eigen bonnetjes inzien, indienen, wijzigen
                var receipt = new Permission() { Name = "receipt.submit", MemberCreatedId = 1 };

                // Mag receipts goed/afkeuren
                var receiptApprove = new Permission() { Name = "receipt.approve", MemberCreatedId = 1 };

                // Mag receipts uitbetalen
                var receiptPay = new Permission() { Name = "receipt.pay", MemberCreatedId = 1 };

                var permissions = new List<Permission>()
                {
                    order, orderProduct, // Team A
                    receipt, receiptApprove, receiptPay, // Team C
                };

                db.Permission.AddRange(permissions);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of permissions: {rows}");

                var memberPermissions = new List<MemberPermission>()
                {
                    // Guest member mag alle basisdingen
                    new MemberPermission() { MemberId = Members["Gast"]!.Id, Permission = order, MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = Members["Gast"]!.Id, Permission = receipt, MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = Members["Product"]!.Id, Permission = orderProduct, MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = Members["Goedkeur"]!.Id, Permission = receiptApprove, MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = Members["Betaal"]!.Id, Permission = receiptPay, MemberCreatedId = 1 },
                };

                db.MemberPermission.AddRange(memberPermissions);

                rows = db.SaveChanges();
                System.Console.WriteLine($"Number of member permissions: {rows}");

            }
        }
        #endregion

        #region Receipt

        private void CreateTestDataCostCentre()
        {
            System.Console.WriteLine("CREATE COST CENTRES");

            using (var db = CreateContext())
            {
                var costCentre = new CostCentre() { Name = "Algemene middelen", MemberCreatedId = 1 };

                db.CostCentre.Add(costCentre);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of cost centres: {rows}");
            }
        }

        #endregion

        #region Order

        private void CreateTestDataProduct()
        {
            System.Console.WriteLine("CREATE PRODUCTS");

            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "admin").FirstOrDefault()!;

                var products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Duifis Scharrelnootjes",
                        Color = "#FF0000",
                        Icon = "fas fa-bowl",
                        Price = 3.45,
                        PriceQuantity = 1,
                        MemberCreated = member,
                    },
                    new Product()
                    {
                        Name = "Vorta Cola",
                        MemberCreated = member,
                        Color = "#FFF000",
                        Icon = "fas fa-bottle",
                        Price = 3.45,
                        PriceQuantity = 1,
                    },
                    new Product() {
                        Name = "Jasmijn Thee",
                        Color = "#FF00F0",
                        Icon = "fas fa-cup",
                        Price = 3.45,
                        PriceQuantity = 1,
                        MemberCreated = member,
                    }
                };
                db.Product.AddRange(products);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of rows: {rows}");
            }
        }

        private void CreateTestDataStock()
        {
            System.Console.WriteLine("CREATE STOCK");

            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "admin").FirstOrDefault()!;

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

        private void CreateTestDataBill()
        {
            System.Console.WriteLine("CREATE BILLS");

            using (var db = CreateContext())
            {
                var member = Members["Admin"];
                var member1 = Members["Gast"];
                var member2 = Members["Product"];

                var billToAdd = new List<Bill>()
                {
                    new Bill() { MemberCreatedId = member.Id, IsGuest = false, Member = member1 },
                    new Bill() { MemberCreatedId = member.Id, IsGuest = false, Member = member2 },
                    new Bill()
                    {
                        MemberCreatedId = member.Id, IsGuest = true, Name = "Neefje van Donald",
                        Note = "Meteen betaald in Duckaten"
                    }
                };

                db.Bills.AddRange(billToAdd);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of bills: {rows}");
            }
        }

        private void CreateTestDataLine()
        {
            System.Console.WriteLine("CREATE BILL LINES");

            using (var db = CreateContext())
            {
                var member = Members["Admin"];
                var member1 = Members["Gast"];
                var member2 = Members["Product"];

                var bill1 = db.Bills.Where(b => b.MemberId == member1.Id).FirstOrDefault()!;
                var bill2 = db.Bills.Where(b => b.MemberId == member2.Id).FirstOrDefault()!;
                var bill3 = db.Bills.Where(b => b.IsGuest).FirstOrDefault()!;

                var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
                var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

                var linesToAdd = new List<Line>()
                {
                    new Line() { Bill = bill1, Product = product1!, Quantity = 1, MemberCreated = member },
                    new Line() { Bill = bill2, Product = product2!, Quantity = 2, MemberCreated = member },
                    new Line() { Bill = bill3, Product = product1!, Quantity = 3, MemberCreated = member },
                    new Line() { Bill = bill3, Product = product2!, Quantity = 4, MemberCreated = member }
                };

                db.OrderLines.AddRange(linesToAdd);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of bill lines: {rows}");
            }
        }

        #endregion
    }
}