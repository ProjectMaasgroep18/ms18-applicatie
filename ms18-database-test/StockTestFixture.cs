using Maasgroep.Database.Order;

namespace Maasgroep.Database.Test
{
	public class StockTestFixture
	{
		private const string ConnectionString = @"UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=MaasgroepTest;Pooling=true";

		private static readonly object _lock = new();
		private static bool _databaseInitialized;

		public StockTestFixture()
		{
			lock (_lock)
			{
				if (!_databaseInitialized)
				{
					InitDatabase();

					// Order
					CreateTestDataProduct();
					CreateTestDataStock();
					CreateTestDataProductPrice();
					CreateTestDataBill();
					CreateTestDataLine();

					_databaseInitialized = true;
				}
			}
		}

		public StockContext CreateContext()
			=> new StockContext(ConnectionString);

		public MemberContext CreateMemberContext() 
			=> new MemberContext(ConnectionString);

		private void InitDatabase()
		{
			using (var context = CreateContext())
			{
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();

				context.SaveChanges();
			}
		}

		#region Order

		private void CreateTestDataProduct()
		{
			//Member member = new Member();

			//using (var db = CreateMemberContext())
			//{
			//	member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
			//}

			//using (var db = CreateContext())
			//{
			//	var products = new List<Product>()
			//	{
			//		new Product() { Name = "Duifis Scharrelnootjes", MemberCreated = member }
			//	,   new Product() { Name = "Vorta Cola", MemberCreated = member }
			//	,   new Product() { Name = "Jasmijn Thee", MemberCreated = member }
			//	};

			//	db.Product.AddRange(products);

			//	var rows = db.SaveChanges();
			//	Console.WriteLine($"Number of rows: {rows}");
			//}
		}

		private void CreateTestDataStock()
		{
			//Member member = new Member();

			//using (var db = CreateMemberContext())
			//{
			//	member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
			//}

			//using (var db = CreateContext())
			//{
			//	var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
			//	var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

			//	var stockToAdd = new List<Stock>()
			//	{
			//		new Stock() { MemberCreated = member, Product = product1, Quantity = 5 }
			//	,   new Stock() { MemberCreated = member, Product = product2, Quantity = 10 }
			//	};

			//	db.Stock.AddRange(stockToAdd);

			//	var rows = db.SaveChanges();
			//	Console.WriteLine($"Number of rows: {rows}");
			//}
		}

		private void CreateTestDataProductPrice()
		{
			//Member member = new Member();

			//using (var db = CreateMemberContext())
			//{
			//	member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
			//}

			//using (var db = CreateContext())
			//{
			//	var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
			//	var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

			//	var priceToAdd = new List<ProductPrice>()
			//	{
			//		new ProductPrice() { MemberCreated = member, Product = product1, Price = 5.5m }
			//	,   new ProductPrice() { MemberCreated = member, Product = product2, Price = 2.5m }
			//	};

			//	db.ProductPrice.AddRange(priceToAdd);

			//	var rows = db.SaveChanges();
			//	Console.WriteLine($"Number of rows: {rows}");
			//}
		}

		private void CreateTestDataBill()
		{
			//Member member = new Member();
			//Member member1 = new Member();
			//Member member2 = new Member();

			//using (var db = CreateMemberContext())
			//{
			//	member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
			//	member1 = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
			//	member2 = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;
			//}

			//using (var db = CreateContext())
			//{
			//	var billToAdd = new List<Bill>()
			//	{
			//		new Bill() { MemberCreated = member, IsGuest = false, Member = member1 }
			//	,   new Bill() { MemberCreated = member, IsGuest = false, Member = member2 }
			//	,   new Bill() { MemberCreated = member, IsGuest = true, Name = "Neefje van Donald", Note = "Meteen betaald in Duckaten" }
			//	};

			//	db.Bills.AddRange(billToAdd);

			//	var rows = db.SaveChanges();
			//	Console.WriteLine($"Number of rows: {rows}");
			//}
		}

		private void CreateTestDataLine()
		{
			//Member member = new Member();
			//Member member1 = new Member();
			//Member member2 = new Member();

			//using (var db = CreateMemberContext())
			//{
			//	member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
			//	member1 = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
			//	member2 = db.Member.Where(m => m.Name == "da Gama").FirstOrDefault()!;
			//}

			//using (var db = CreateContext())
			//{
			//	var bill1 = db.Bills.Where(b => b.MemberId == member1.Id).FirstOrDefault()!;
			//	var bill2 = db.Bills.Where(b => b.MemberId == member2.Id).FirstOrDefault()!;
			//	var bill3 = db.Bills.Where(b => b.IsGuest).FirstOrDefault()!;

			//	var product1 = db.Product.Where(p => p.Name == "Duifis Scharrelnootjes").FirstOrDefault();
			//	var product2 = db.Product.Where(p => p.Name == "Vorta Cola").FirstOrDefault();

			//	var linesToAdd = new List<Line>()
			//	{
			//		new Line() { Bill = bill1, Product = product1, Quantity = 1, MemberCreated = member }
			//	,   new Line() { Bill = bill2, Product = product2, Quantity = 2, MemberCreated = member }
			//	,   new Line() { Bill = bill3, Product = product1, Quantity = 3, MemberCreated = member }
			//	,   new Line() { Bill = bill3, Product = product2, Quantity = 4, MemberCreated = member }
			//	};

			//	db.OrderLines.AddRange(linesToAdd);

			//	var rows = db.SaveChanges();
			//	Console.WriteLine($"Number of rows: {rows}");
			//}
		}

		#endregion
	}
}
