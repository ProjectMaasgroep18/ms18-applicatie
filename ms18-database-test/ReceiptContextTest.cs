using FluentAssertions;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Test;
using Microsoft.EntityFrameworkCore;

namespace ms18_database_test
{
	public class ReceiptContextTest : IClassFixture<MaasgroepTestFixture>
	{
		public ReceiptContextTest(MaasgroepTestFixture fixture) => Fixture = fixture;

		public MaasgroepTestFixture Fixture { get; }

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

				_ = result.Entity.Id.Should().NotBe(0);
			}
			finally
			{
				sut.Database.RollbackTransaction();
			}
			
			
			
		}


		private Receipt GetValidReceipt()
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
