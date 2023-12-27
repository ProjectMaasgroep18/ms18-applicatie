using Maasgroep.Database;
using Maasgroep.Database.Admin;
using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;
using Microsoft.EntityFrameworkCore;

namespace Maasgroep.Console
{
    internal class DatabaseTestData
    {
        protected Dictionary<string, Member> CurrentMembers = new();

        internal void CreateTestData()
        {
            try {
                // ADMIN + PERMISSIONS data
                CreateTestDataMember();
                CreateTestDataPermissions();
            } catch (DbUpdateException e) {
                // Waarschijnlijk duplicate keys (bestond dan al)
                System.Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }
            try {
                // RECEIPT (Cost Centre) data   
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
                CurrentMembers = db.Member.ToDictionary(m => m.Name, m => m);
                var verySafePassword = GetPasswordHash("123456");
                var emptyPassword = GetPasswordHash("");
                var admin = CurrentMembers.ContainsKey("Admin") ? CurrentMembers["Admin"] : new Member() { Id = 1, Name = "Admin", Email = "admin@example.com", Color = "#3366FF", Password = verySafePassword };
                var members = new Dictionary<string, Member>
                {
                    ["Admin"] = admin,
                    ["Lid"] = new Member() { Name = "Lid", MemberCreated = admin, Email = "lid@example.com", Color = "#FF2222", Password = verySafePassword },
                 
                    // Team B
                    ["Gast"] = new Member() { Name = "Gast", MemberCreated = admin, Email = "gast@example.com", Color = "#ABCDEF", IsGuest = true, Password = emptyPassword },
                    ["Bar"] = new Member() { Name = "Bar", MemberCreated = admin, Email = "bar@example.com", Color = "#00CCFF", Password = verySafePassword },
                    ["Product"] = new Member() { Name = "Product", MemberCreated = admin, Email = "product@example.com", Color = "#FFCC00", Password = verySafePassword },

                    // Team C
                    ["Goedkeur"] = new Member() { Name = "Goedkeur", MemberCreated = admin, Email = "goedkeur@example.com", Color = "#00CC00", Password = verySafePassword },
                    ["Betaal"] = new Member() { Name = "Betaal", MemberCreated = admin, Email = "betaal@example.com", Color = "#CC00FF", Password = verySafePassword },
                };

                foreach (var member in members) {
                    if (!CurrentMembers.ContainsKey(member.Key)) {
                        db.Member.Add(member.Value);
                        CurrentMembers[member.Key] = member.Value;
                    }
                }

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of members: {rows}");
            }
        }

        private void CreateTestDataPermissions()
        {
            System.Console.WriteLine("CREATE PERMISSIONS");

            using (var db = CreateContext())
            {
                var currentPermissions = db.Permission.ToDictionary(p => p.Name, p => p);
                var permissions = new List<string>()
                {
                    "admin", // Speciale admin permission (mag alles)
                    "order", // Toegang tot het "order" gedeelte: producten zien, plaatsen van bestellingen, eigen bestellingen zien
                    "order.view", // Mag geplaatste bestellingen zien;
                    "order.product", // Mag producten toevoegen, wijzigen
                    "receipt", // Toegang tot het "receipt" gedeelte: eigen bonnetjes inzien, indienen, wijzigen
                    "receipt.approve", // Mag receipts zien en goed/afkeuren
                    "receipt.pay", // Mag receipts zien en uitbetalen
                }.ToDictionary(name => name, name => new Permission() { Name = name, MemberCreatedId = 1 });

                foreach (var permission in permissions) {
                    if (!currentPermissions.ContainsKey(permission.Key)) {
                        db.Permission.Add(permission.Value);
                        currentPermissions[permission.Key] = permission.Value;
                    }
                }

                var rows = db.SaveChanges();
                System.Console.WriteLine($"Number of permissions: {rows}");

                var currentMemberPermissions = db.MemberPermission.ToDictionary(mp => $"{mp.MemberId}/{mp.PermissionId}", mp => mp);
                var memberPermissions = new List<MemberPermission>()
                {
                    new MemberPermission() { MemberId = CurrentMembers["Admin"]!.Id, Permission = currentPermissions["admin"], MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = CurrentMembers["Lid"]!.Id, Permission = currentPermissions["order"], MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = CurrentMembers["Lid"]!.Id, Permission = currentPermissions["receipt"], MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = CurrentMembers["Gast"]!.Id, Permission = currentPermissions["order"], MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = CurrentMembers["Bar"]!.Id, Permission = currentPermissions["order.view"], MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = CurrentMembers["Product"]!.Id, Permission = currentPermissions["order.product"], MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = CurrentMembers["Goedkeur"]!.Id, Permission = currentPermissions["receipt.approve"], MemberCreatedId = 1 },
                    new MemberPermission() { MemberId = CurrentMembers["Betaal"]!.Id, Permission = currentPermissions["receipt.pay"], MemberCreatedId = 1 },
                };

                foreach (var memberPermission in memberPermissions) {
                    var exists = currentMemberPermissions.ContainsKey($"{memberPermission.MemberId}/{memberPermission.Permission.Id}");
                    if (!exists) {
                        db.MemberPermission.Add(memberPermission);
                        currentMemberPermissions[$"{memberPermission.MemberId}/{memberPermission.Permission.Id}"] = memberPermission;
                    }
                }

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
                if (!db.CostCentre.Any())
                {
                    var costCentre = new CostCentre() { Name = "Algemene middelen", MemberCreatedId = 1 };
                    db.CostCentre.Add(costCentre);
                    var rows = db.SaveChanges();
                    System.Console.WriteLine($"Number of cost centres: {rows}");
                }
                else
                {
                    System.Console.WriteLine("There is already at least one cost centre");
                }
            }
        }

        #endregion

        #region Order

        private void CreateTestDataProduct()
        {
            System.Console.WriteLine("CREATE PRODUCTS");

            using (var db = CreateContext())
            {
                if (!db.Product.Any())
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
                else
                {
                    System.Console.WriteLine("There is already at least one product");
                }
            }
        }

        private void CreateTestDataStock()
        {
            System.Console.WriteLine("CREATE STOCK");

            using (var db = CreateContext())
            {
                if (!db.Stock.Any())
                {
                    var member = CurrentMembers["Admin"];

                    var product1 = db.Product.Take(1).FirstOrDefault();
                    var product2 = db.Product.Skip(1).Take(1).FirstOrDefault();

                    if (product1 != null && product2 != null)
                    {
                        var stockToAdd = new List<Stock>()
                        {
                            new Stock() { MemberCreated = member, Product = product1, Quantity = 5 },
                            new Stock() { MemberCreated = member, Product = product2, Quantity = 10 }
                        };
                        db.Stock.AddRange(stockToAdd);
                    }
                    else
                    {
                        System.Console.WriteLine("The default products were not found");
                    }
                    var rows = db.SaveChanges();
                    System.Console.WriteLine($"Number of rows: {rows}");
                }
                else
                {
                    System.Console.WriteLine("There is already at least one stock");
                }
            }
        }

        private void CreateTestDataBill()
        {
            System.Console.WriteLine("CREATE BILLS");

            using (var db = CreateContext())
            {
                if (!db.Bills.Any())
                {
                    var member1 = CurrentMembers["Lid"];
                    var member2 = CurrentMembers["Product"];
                    var memberGuest = CurrentMembers["Gast"];

                    var billToAdd = new List<Bill>()
                    {
                        new Bill() { MemberCreatedId = member1.Id, Name = member1.Name, Email = member1.Email, IsGuest = false },
                        new Bill() { MemberCreatedId = member2.Id, Name = member2.Name, Email = member2.Email, IsGuest = false },
                        new Bill()
                        {
                            MemberCreatedId = memberGuest.Id,
                            IsGuest = true,
                            Name = "Neefje van Donald",
                            Email = memberGuest.Email,
                            Note = "Meteen betaald in Duckaten",
                        }
                    };

                    db.Bills.AddRange(billToAdd);
                    var rows = db.SaveChanges();
                    System.Console.WriteLine($"Number of bills: {rows}");
                }
                else
                {
                    System.Console.WriteLine("There is already at least one bill");
                }
            }
        }

        private void CreateTestDataLine()
        {
            System.Console.WriteLine("CREATE BILL LINES");

            using (var db = CreateContext())
            {
                if (!db.OrderLines.Any())
                {
                    var member1 = CurrentMembers["Lid"];
                    var member2 = CurrentMembers["Product"];

                    var bill1 = db.Bills.Where(b => b.MemberCreatedId == member1.Id).FirstOrDefault();
                    var bill2 = db.Bills.Where(b => b.MemberCreatedId == member2.Id).FirstOrDefault();
                    var bill3 = db.Bills.Where(b => b.IsGuest).FirstOrDefault();

                    if (bill1 != null && bill2 != null && bill3 != null)
                    {
                        var product1 = db.Product.Take(1).FirstOrDefault();
                        var product2 = db.Product.Skip(1).Take(1).FirstOrDefault();

                        if (product2 == null)
                            product2 = product1;

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
                    else
                    {
                        System.Console.WriteLine("The default bills were not found");
                    }
                }
                else
                {
                    System.Console.WriteLine("There is already at least one bill line");
                }
            }
        }

        #endregion
    }
}