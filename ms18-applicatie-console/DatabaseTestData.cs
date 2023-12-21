using Maasgroep.Database;
using Maasgroep.Database.Admin;
using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;
using Microsoft.EntityFrameworkCore;

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
            } catch (DbUpdateException e) {
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
            } catch (DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }
            try {
                // RECEIPT data   
                CreateTestDataCostCentre();
            } catch (DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }
            try {
                // PRODUCT data
                CreateTestDataProduct();
                CreateTestDataStock();
                CreateTestDataBill();
                CreateTestDataLine();
            } catch (DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }    
        }

        private MaasgroepContext CreateContext()
            => new();

        private string GetPasswordHash(string password)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = System.Security.Cryptography.SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

        #region Member_Admin

        private void CreateTestDataMember()
        {
            System.Console.WriteLine("CREATE MEMBERS");

            using (var db = CreateContext())
            {
                var verySafePassword = GetPasswordHash("123456");
                var admin = Members.ContainsKey("Admin") ? Members["Admin"] : new Member() { Name = "Admin", Email = "admin@example.com", Password = verySafePassword };
                Members = new Dictionary<string, Member>
                {
                    ["Admin"] = admin,
                    ["Lid"] = new Member() { Name = "Lid", MemberCreated = admin, Email = "lid@example.com", Password = verySafePassword },
                 
                    // Team B
                    ["Product"] = new Member() { Name = "Product", MemberCreated = admin, Email = "product@example.com", Password = verySafePassword },

                    // Team C
                    ["Goedkeur"] = new Member() { Name = "Goedkeur", MemberCreated = admin, Email = "goedkeur@example.com", Password = verySafePassword },
                    ["Betaal"] = new Member() { Name = "Betaal", MemberCreated = admin, Email = "betaal@example.com", Password = verySafePassword },
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
                // Speciale admin permission (mag alles)
                var admin = new Permission() { Name = "admin", MemberCreatedId = 1 };
                
                // Toegang tot het order gedeelte: producten zien; bestellingen zien;
                var order = new Permission() { Name = "order.view", MemberCreatedId = 1 };

                // Mag producten toevoegen, wijzigen
                var orderProduct = new Permission() { Name = "order.product", MemberCreatedId = 1 };

                // Toegang tot het "receipt" gedeelte: eigen bonnetjes inzien, indienen, wijzigen
                var receipt = new Permission() { Name = "receipt", MemberCreatedId = 1 };

                // Mag receipts goed/afkeuren
                var receiptApprove = new Permission() { Name = "receipt.approve", MemberCreatedId = 1 };

                // Mag receipts uitbetalen
                var receiptPay = new Permission() { Name = "receipt.pay", MemberCreatedId = 1 };

                var permissions = new List<Permission>()
                {
                    admin,
                    order, orderProduct, // Team B
                    receipt, receiptApprove, receiptPay, // Team C
                };

                db.Permission.AddRange(permissions);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of permissions: {rows}");

                var memberPermissions = new List<MemberPermission>()
                {
                    new MemberPermission() { MemberId = Members["Admin"]!.Id, Permission = admin, MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = Members["Lid"]!.Id, Permission = order, MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = Members["Lid"]!.Id, Permission = receipt, MemberCreatedId = 1 },
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
                    new Stock() { MemberCreated = member, Product = product1!, Quantity = 5 },
                    new Stock() { MemberCreated = member, Product = product2!, Quantity = 10 }
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
                var member1 = Members["Lid"];
                var member2 = Members["Product"];

                var billToAdd = new List<Bill>()
                {
                    new Bill() { MemberCreatedId = member1.Id, IsGuest = false },
                    new Bill() { MemberCreatedId = member2.Id, IsGuest = false },
                    new Bill()
                    {
                        IsGuest = true,
                        Name = "Neefje van Donald",
                        Note = "Meteen betaald in Duckaten",
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
                var member1 = Members["Lid"];
                var member2 = Members["Product"];

                var bill1 = db.Bills.Where(b => b.MemberCreatedId == member1.Id).FirstOrDefault()!;
                var bill2 = db.Bills.Where(b => b.MemberCreatedId == member2.Id).FirstOrDefault()!;
                var bill3 = db.Bills.Where(b => b.IsGuest).FirstOrDefault()!;

                var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
                var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

                var linesToAdd = new List<Line>()
                {
                    new Line() { Bill = bill1, Product = product1!, Name = product1!.Name, Price = product1!.Price, Quantity = 1, Amount = product1!.Price * 1, MemberCreatedId = 1 },
                    new Line() { Bill = bill2, Product = product2!, Name = product2!.Name, Price = product2!.Price, Quantity = 2, Amount = product2!.Price * 2, MemberCreatedId = 1 },
                    new Line() { Bill = bill3, Product = product1!, Name = product1!.Name, Price = product1!.Price, Quantity = 3, Amount = product1!.Price * 3, MemberCreatedId = 1 },
                    new Line() { Bill = bill3, Product = product2!, Name = product2!.Name, Price = product2!.Price, Quantity = 4, Amount = product2!.Price * 4, MemberCreatedId = 1 },
                };

                db.OrderLines.AddRange(linesToAdd);

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of bill lines: {rows}");

                bill1.TotalAmount = linesToAdd.Where(line => line.Bill == bill1).Sum(line => line.Amount);
                bill2.TotalAmount = linesToAdd.Where(line => line.Bill == bill2).Sum(line => line.Amount);
                bill3.TotalAmount = linesToAdd.Where(line => line.Bill == bill3).Sum(line => line.Amount);

                db.Bills.Update(bill1);
                db.Bills.Update(bill2);
                db.Bills.Update(bill3);

                rows = db.SaveChanges();
                System.Console.WriteLine($"Number of bills updated: {rows}");
            }
        }

        #endregion
    }
}