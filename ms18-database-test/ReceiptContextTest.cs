using FluentAssertions;
using Maasgroep.Database;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Test;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ms18_database_test
{
	public class ReceiptContextTest : IClassFixture<MaasgroepTestFixture>
	{
		private const string _receiptSeq = "receiptSeq";
		private const string _costCentreSeq = "costCentreSeq";
		private const string _photoSeq = "photoSeq";

		public ReceiptContextTest(MaasgroepTestFixture fixture) => Fixture = fixture;

		public MaasgroepTestFixture Fixture { get; }


		#region Receipt

		[Fact]
		public void Receipt_Add_A_Valid_Receipt_Result_Ok()
		{
			using var sut = Fixture.CreateContext();

			try
			{
				var receipt = GetValidReceipt();

				sut.Database.BeginTransaction();

				var result = sut.Receipt.Add(receipt);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();
				long idOfReceipt = GetLatestSequenceId(sut, _receiptSeq);

				_ = result.Entity.Id.Should().Be(idOfReceipt);
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}
		}

		[Fact]
		public void Receipt_Add_Invalid_Receipt_Note2049Char_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var receipt = GetValidReceipt();
				receipt.Note = GetLongString(2049);

				sut.Database.BeginTransaction();

				var result = sut.Receipt.Add(receipt);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "dit mag dus niet";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("22001: value too long for type character varying(2048)");
		}

		[Fact] 
		public void Receipt_Add_Invalid_Receipt_AmountLessThanZero_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var receipt = GetValidReceipt();
				receipt.Amount = -1.23M;

				sut.Database.BeginTransaction();

				var result = sut.Receipt.Add(receipt);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "dit mag dus niet";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23514: new row for relation \"receipt\" violates check constraint \"CK_receipt_amount\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		[Fact]
		public void Receipt_Add_Invalid_Receipt_CostCentreIdWrong_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var receipt = GetValidReceipt();
				receipt.CostCentreId = -1;

				sut.Database.BeginTransaction();

				var result = sut.Receipt.Add(receipt);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "dit mag dus niet";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23503: insert or update on table \"receipt\" violates foreign key constraint \"FK_receipt_costCentre\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		[Fact]
		public void Receipt_Add_Invalid_Receipt_MemberIdWrong_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var receipt = GetValidReceipt();
				receipt.MemberCreatedId = -1;

				sut.Database.BeginTransaction();

				var result = sut.Receipt.Add(receipt);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "dit mag dus niet";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23503: insert or update on table \"receipt\" violates foreign key constraint \"FK_receipt_memberCreated\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		#endregion

		#region CostCentre

		[Fact]
		public void CostCentre_Add_A_Valid_CostCentre_Result_Ok()
		{
			using var sut = Fixture.CreateContext();

			try
			{
				var costCentre = GetValidCostCentre();

				sut.Database.BeginTransaction();

				var result = sut.CostCentre.Add(costCentre);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();
				long idOfReceipt = GetLatestSequenceId(sut, _costCentreSeq);

				_ = result.Entity.Id.Should().Be(idOfReceipt);
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}
		}

		[Fact]
		public void CostCentre_Add_Invalid_CostCentre_Name257Char_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var costCentre = GetValidCostCentre();
				costCentre.Name = GetLongString(257);

				sut.Database.BeginTransaction();

				var result = sut.CostCentre.Add(costCentre);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException ex)
			{
				actual = ex;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("22001: value too long for type character varying(256)");
		}

		[Fact]
		public void CostCentre_Add_Invalid_CostCentre_DuplicateName_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var costCentre = GetValidCostCentre();
				costCentre.Name = "Bestuur Maasgroep";

				sut.Database.BeginTransaction();

				var result = sut.CostCentre.Add(costCentre);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException ex)
			{
				actual = ex;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23505: duplicate key value violates unique constraint \"IX_costCentre_Name\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		[Fact]
		public void CostCentre_Add_Invalid_CostCentre_MemberIdWrong_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var costCentre = GetValidCostCentre();
				costCentre.MemberCreatedId = -1;

				sut.Database.BeginTransaction();

				var result = sut.CostCentre.Add(costCentre);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException ex)
			{
				actual = ex;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23503: insert or update on table \"costCentre\" violates foreign key constraint \"FK_costCentre_memberCreated\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		#endregion

		#region ReceiptApproval

		[Fact]
		public void ReceiptApproval_Add_Valid_Approval_Ok()
		{
			using var sut = Fixture.CreateContext();

			int countActual = 0;

			try
			{
				var approval = GetValidApproval();

				sut.Database.BeginTransaction();

				var result = sut.ReceiptApproval.Add(approval);
				_ = result.State.Should().Be(EntityState.Added);

				countActual = sut.SaveChanges();	
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = countActual.Should().Be(1);
		}

		[Fact]
		public void ReceiptApproval_Add_Invalid_Approval_Note2049Char_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var approval = GetValidApproval();
				approval.Note = GetLongString(2049);

				sut.Database.BeginTransaction();

				var result = sut.ReceiptApproval.Add(approval);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().NotBeNull();
			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("22001: value too long for type character varying(2048)");
		}

		[Fact]
		public void ReceiptApproval_Add_Invalid_Approval_ReceiptIdWrong_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var approval = GetValidApproval();
				approval.ReceiptId = -1;

				sut.Database.BeginTransaction();

				var result = sut.ReceiptApproval.Add(approval);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().NotBeNull();
			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23503: insert or update on table \"approval\" violates foreign key constraint \"FK_receiptApproval_receipt\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		[Fact]
		public void ReceiptApproval_Add_Invalid_Approval_MemberIdWrong_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var approval = GetValidApproval();
				approval.MemberCreatedId = -1;

				sut.Database.BeginTransaction();

				var result = sut.ReceiptApproval.Add(approval);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().NotBeNull();
			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23503: insert or update on table \"approval\" violates foreign key constraint \"FK_receiptApproval_memberCreated\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		#endregion

		#region Photo

		[Fact]
		public void Photo_Add_Valid_Photo_Result_Ok()
		{
			using var sut = Fixture.CreateContext();

			try
			{
				var photo = GetValidPhoto();

				sut.Database.BeginTransaction();

				var result = sut.ReceiptPhoto.Add(photo);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();
				long idOfPhoto = GetLatestSequenceId(sut, _photoSeq);

				_ = result.Entity.Id.Should().Be(idOfPhoto);
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}
		}

		[Fact]
		public void Photo_Add_Invalid_Photo_FileExtension257_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var photo = GetValidPhoto();
				photo.fileExtension = GetLongString(257);

				sut.Database.BeginTransaction();

				var result = sut.ReceiptPhoto.Add(photo);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().NotBeNull();
			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("22001: value too long for type character varying(256)");
		}

		[Fact]
		public void Photo_Add_Invalid_Photo_FileName2049_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var photo = GetValidPhoto();
				photo.fileName = GetLongString(2049);

				sut.Database.BeginTransaction();

				var result = sut.ReceiptPhoto.Add(photo);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().NotBeNull();
			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("22001: value too long for type character varying(2048)");
		}

		[Fact]
		public void Photo_Add_Invalid_Photo_MemberIdWrong_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var photo = GetValidPhoto();
				photo.MemberCreatedId = -1;

				sut.Database.BeginTransaction();

				var result = sut.ReceiptPhoto.Add(photo);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().NotBeNull();
			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23503: insert or update on table \"photo\" violates foreign key constraint \"FK_photo_memberCreated\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		[Fact]
		public void Photo_Add_Invalid_Photo_ReceiptIdWrong_Error()
		{
			using var sut = Fixture.CreateContext();

			object actual = new();

			try
			{
				var photo = GetValidPhoto();
				photo.ReceiptId = -1;

				sut.Database.BeginTransaction();

				var result = sut.ReceiptPhoto.Add(photo);
				_ = result.State.Should().Be(EntityState.Added);

				sut.SaveChanges();

				actual = "mag hier niet komen";
			}
			catch (DbUpdateException expected)
			{
				actual = expected;
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}

			_ = actual.Should().NotBeNull();
			_ = actual.Should().BeOfType<DbUpdateException>();
			_ = actual.As<DbUpdateException>().InnerException.Should().NotBeNull();
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("23503: insert or update on table \"photo\" violates foreign key constraint \"FK_photo_receipt\"\r\n\r\nDETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.");
		}

		#endregion



		private string GetLongString(int length)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < length; i++)
				sb.Append("x");

			return sb.ToString();
		}

		private static long GetLatestSequenceId(MaasgroepContext sut, string seqName)
		{
			switch (seqName)
			{
				case _costCentreSeq:
					return sut.Database.SqlQuery<long>($"select currval('receipt.\"costCentreSeq\"'::regclass)").ToList().FirstOrDefault();
				case _photoSeq:
					return sut.Database.SqlQuery<long>($"select currval('receipt.\"photoSeq\"'::regclass)").ToList().FirstOrDefault();
				case _receiptSeq:
					return sut.Database.SqlQuery<long>($"select currval('receipt.\"receiptSeq\"'::regclass)").ToList().FirstOrDefault();
				default:
					return -1;
			}
		}

		private static Receipt GetValidReceipt()
		{
			return new Receipt()
			{
				Amount = 9876543.21M,
				CostCentreId = 1,
				ReceiptStatus = "Goedgekeurd",
				Location = "",
				Note = "Lieve Willemijn, wat ik je altijd al heb willen zeggen...",
				MemberCreatedId = 1
			};
		}

		private static CostCentre GetValidCostCentre()
		{
			return new CostCentre()
			{
				Name = "Andermans bankrekening",
				MemberCreatedId = 1
			};
		}

		private static ReceiptApproval GetValidApproval()
		{
			return new ReceiptApproval()
			{
				ReceiptId = 1,
				Note = "Puik werk! Helemaal mee eens!",
				Approved = true,
				MemberCreatedId = 1
			};
		}

		private static ReceiptPhoto GetValidPhoto()
		{
			return new ReceiptPhoto() {
				ReceiptId = 1,
				fileExtension = "png",
				fileName = "kijk die serieuze extensie",
				Base64Image = "bliep bloep",
				Location = "digitaal",
				MemberCreatedId = 1
			};
		}
	}
}
