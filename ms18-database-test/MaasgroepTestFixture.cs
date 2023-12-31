﻿using Maasgroep.Database.Admin;
using Maasgroep.Database.Orders;
using Maasgroep.Database.Receipts;

namespace Maasgroep.Database.Test
{
	public class MaasgroepTestFixture
	{
		private const string ConnectionString = @"UserID=postgres;Password=postgres;Host=localhost;port=5432;Database=MaasgroepTest;Pooling=true";

		private static readonly object _lock = new();
		private static bool _databaseInitialized;

		public MaasgroepTestFixture()
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
					CreateTestDataReceiptHistory();
					CreateTestDataReceiptPhoto();

					// Order
					//CreateTestDataProduct();
					//CreateTestDataStock();
					//CreateTestDataProductPrice();
					//CreateTestDataBill();
					//CreateTestDataLine();

					_databaseInitialized = true;
				}
			}
		}

		public MaasgroepContext CreateContext()
			=> new MaasgroepContext(ConnectionString);

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
			using (var db = CreateContext())
			{
				var members = new List<Member>()
				{
					new Member() { Name = "Borgia"}
				};

				db.Member.AddRange(members);

				db.SaveChanges();

				var borgia = db.Member.FirstOrDefault()!;

				members = new List<Member>()
				{
					new Member() { Name = "da Gama", MemberCreated = borgia }
				,   new Member() { Name = "Albuquerque", MemberCreated = borgia }
				};

				borgia.MemberCreated = borgia;
				borgia.MemberModified = borgia;
				borgia.DateTimeModified = DateTime.UtcNow;

				db.Member.AddRange(members);

				db.SaveChanges();
			}
		}
		private void CreateTestDataPermissions()
		{
			using (var db = CreateContext())
			{
				var borgia = db.Member.FirstOrDefault()!;

				var permissions = new List<Permission>()
				{
					new Permission() { Name = "receipt.approve", MemberCreated = borgia}
				,   new Permission() { Name = "receipt.reject", MemberCreated = borgia}
				,   new Permission() { Name = "receipt.handIn", MemberCreated = borgia}
				,   new Permission() { Name = "receipt.payOut", MemberCreated = borgia}
				};

				db.Permission.AddRange(permissions);

				db.SaveChanges();
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
					new MemberPermission() { Member = daGama, Permission = approve, MemberCreated = borgia }
				,   new MemberPermission() { Member = daGama, Permission = reject, MemberCreated = borgia }
				,   new MemberPermission() { Member = alb, Permission = handIn, MemberCreated = borgia }
				};

				db.MemberPermission.AddRange(memberPermissions);

				db.SaveChanges();
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
					new CostCentre() { Name = "Bestuur Maasgroep", MemberCreated = member }
				,   new CostCentre() { Name = "Penningmeester", MemberCreated = member }
				,   new CostCentre() { Name = "Moeder van Joopie", MemberCreated = member }
				};

				db.CostCentre.AddRange(costCentres);

				db.SaveChanges();
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
							new Receipt()   { MemberCreated = member, ReceiptStatus = "Ingediend"
											, CostCentre = costCentre1, Amount = 1.11M, Note = "Schroeven voor kapotte sloep"
											}
						,   new Receipt()   { MemberCreated = member, ReceiptStatus = "Goedgekeurd"
											, CostCentre = costCentre2, Amount = 2.22M, Note = "Knakworstjes in de bonus"
											}
						,   new Receipt()   { MemberCreated = member, ReceiptStatus = "Afgekeurd"
											, CostCentre = costCentre1, Amount = 3.33M, Note = "Handboek Woudlopers"
											}
						,   new Receipt()   { MemberCreated = member, ReceiptStatus = "Uitbetaald"
											, CostCentre = costCentre1, Amount = 4.44M, Note = "Losgeld voor piraten van de zeven zeeen"
											, DateTimeDeleted = DateTime.UtcNow, MemberDeleted = member
											}
						};

				db.Receipt.AddRange(receipts);

				db.SaveChanges();
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

				db.SaveChanges();
			}
		}
		private void CreateTestDataReceiptPhoto()
		{
			using (var db = CreateContext())
			{
				var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
				var costCentre1 = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
				var costCentre2 = db.CostCentre.Where(cc => cc.Name == "Penningmeester").FirstOrDefault()!;

				var receipt = db.Receipt.Where(r => r.Note == "Schroeven voor kapotte sloep").FirstOrDefault();

				var receiptPhotos = new List<ReceiptPhoto>()
						{
							new ReceiptPhoto() { 
								Receipt = receipt
							,   ReceiptId = receipt.Id 
							,	Base64Image = "fake data"
							,	FileExtension = "png"
							,	FileName = "Bootje"
							,	MemberCreated = member
							,	MemberCreatedId = member.Id
							},
							new ReceiptPhoto() {
								Receipt = receipt
							,   ReceiptId = receipt.Id
							,   Base64Image = "fake data"
							,   FileExtension = "png"
							,   FileName = "Kraaiennest"
							,   MemberCreated = member
							,   MemberCreatedId = member.Id
							}
						};

				db.ReceiptPhoto.AddRange(receiptPhotos);

				db.SaveChanges();
			}
		}


		private void CreateTestDataReceiptHistory()
		{
			using (var db = CreateContext())
			{
				var member = db.Member.Where(m => m.Name == "Borgia").FirstOrDefault()!;
				var costCentre1 = db.CostCentre.Where(cc => cc.Name == "Moeder van Joopie").FirstOrDefault()!;
				var costCentre2 = db.CostCentre.Where(cc => cc.Name == "Penningmeester").FirstOrDefault()!;

				var receipts = new List<ReceiptHistory>()
						{
							new ReceiptHistory()   { ReceiptId = 1, MemberCreatedId = member.Id, ReceiptStatus = "Ingediend"
											, CostCentreId = costCentre1.Id, Amount = 1.11M, Note = "Schroeven voor kapotte sloep histo-rietje"
											}
						,   new ReceiptHistory()   { ReceiptId = 2, MemberCreatedId = member.Id, ReceiptStatus = "Goedgekeurd"
											, CostCentreId = costCentre2.Id, Amount = 2.22M, Note = "Knakworstjes in de bonus gisteren"
											}
						,   new ReceiptHistory()   { ReceiptId = 3, MemberCreatedId = member.Id, ReceiptStatus = "Afgekeurd"
											, CostCentreId = costCentre1.Id, Amount = 3.33M, Note = "Handboek Woudlopers van vorig jaar"
											}
						};
				db.ReceiptHistory.AddRange(receipts);

				db.SaveChanges();
			}
		}
		#endregion

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
