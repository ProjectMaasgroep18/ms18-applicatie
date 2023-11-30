using Maasgroep.Database.Order;

namespace Maasgroep.Database.Orders
{
    public class OrderRepository : IOrderRepository
    {
        public void AanmakenTestData()
        {
            CreateTestDataProduct();
            CreateTestDataStock();
            CreateTestDataProductPrice();
            CreateTestDataBill();
            CreateTestDataLine();
        }

        public MaasgroepContext CreateContext()
        {
            return new MaasgroepContext();
        }

        private void CreateTestDataProduct()
        {
            using (var db = CreateContext())
            {
                var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;

                var products = new List<Product>()
                {
                    new Product() { Name = "Duifis Scharrelnootjes", MemberCreated = member }
                ,   new Product() { Name = "Vorta Cola", MemberCreated = member }
                ,   new Product() { Name = "Jasmijn Thee", MemberCreated = member }
                };

                db.Product.AddRange(products);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
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
                    new Stock() { MemberCreated = member, Product = product1, Quantity = 5 }
                ,   new Stock() { MemberCreated = member, Product = product2, Quantity = 10 }
                };

                db.Stock.AddRange(stockToAdd);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
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
                    new ProductPrice() { MemberCreated = member, Product = product1, Price = 5.5m }
                ,   new ProductPrice() { MemberCreated = member, Product = product2, Price = 2.5m }
                };

                db.ProductPrice.AddRange(priceToAdd);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
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
                    new Bill() { MemberCreated = member, IsGuest = false, Member = member1 }
                ,   new Bill() { MemberCreated = member, IsGuest = false, Member = member2 }
                ,   new Bill() { MemberCreated = member, IsGuest = true, Name = "Neefje van Donald", Note = "Meteen betaald in Duckaten" }
                };

                db.Bills.AddRange(billToAdd);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
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
                    new Line() { Bill = bill1, Product = product1, Quantity = 1, MemberCreated = member }
                ,   new Line() { Bill = bill2, Product = product2, Quantity = 2, MemberCreated = member }
                ,   new Line() { Bill = bill3, Product = product1, Quantity = 3, MemberCreated = member }
                ,   new Line() { Bill = bill3, Product = product2, Quantity = 4, MemberCreated = member }
                };

                db.OrderLines.AddRange(linesToAdd);

                var rows = db.SaveChanges();
                Console.WriteLine($"Number of rows: {rows}");
            }
        }
    }
}
