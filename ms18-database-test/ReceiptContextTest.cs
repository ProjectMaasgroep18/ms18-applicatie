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
				long idOfReceipt = GetLatestReceiptId(sut, _receiptSeq);

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
			_ = actual.As<DbUpdateException>().InnerException!.Message.Should().Be("22001: value too long for type character varying(2048)");
		}

		[Fact]
		public void Receipt_Add_Invalid_Receipt_CostCentreIdWrong_Error()
		{

		}

		[Fact]
		public void Receipt_Add_Invalid_Receipt_MemberIdWrong_Error()
		{

		}


		#endregion

		#region CostCentre

		[Fact]
		public void CostCentre_Add_A_Valid_CostCentre_Result_Ok()
		{
			//_costCentreSeq
		}


		[Fact]
		public void CostCentre_Add_Invalid_CostCentre_Name257Char_Error()
		{

		}

		[Fact]
		public void CostCentre_Add_Invalid_CostCentre_DuplicateName_Error()
		{

		}

		[Fact]
		public void CostCentre_Add_Invalid_CostCentre_MemberIdWrong_Error()
		{

		}

		#endregion

		#region ReceiptApproval

		[Fact]
		public void ReceiptApproval_Add_Valid_Approval_Ok()
		{

		}

		[Fact]
		public void ReceiptApproval_Add_Invalid_Approval_Note2049Char_Error()
		{

		}


		[Fact]
		public void ReceiptApproval_Add_Invalid_Approval_ReceiptIdWrong_Error()
		{

		}

		[Fact]
		public void ReceiptApproval_Add_Invalid_Approval_MemberIdWrong_Error()
		{

		}

		#endregion

		#region Photo

		[Fact]
		public void Photo_Add_Valid_Photo_Result_Ok()
		{

		}

		[Fact]
		public void Photo_Add_Invalid_Photo_FileExtension257_Error()
		{

		}

		[Fact]
		public void Photo_Add_Invalid_Photo_FileName2049_Error()
		{

		}

		[Fact]
		public void Photo_Add_Invalid_Photo_MemberIdWrong_Error()
		{

		}

		[Fact]
		public void Photo_Add_Invalid_Photo_ReceiptIdWrong_Error()
		{

		}

		#endregion




		private string GetLongString(int length)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < length; i++)
				sb.Append("x");

			return sb.ToString();
		}

		private static long GetLatestReceiptId(MaasgroepContext sut, string seqName)
		{
			return sut.Database.SqlQuery<long>($"select currval('receipt.\"{seqName}\"'::regclass)").ToList().FirstOrDefault();
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

	}
}
