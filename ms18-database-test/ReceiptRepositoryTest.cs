using FluentAssertions;
using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Moq;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.DataModels.Receipts;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.Database.Test
{
	// Documentatie
	// https://learn.microsoft.com/en-us/ef/core/testing/
	// https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0

	public class ReceiptRepositoryTest : IClassFixture<MaasgroepTestFixture>
	{
		public ReceiptRepositoryTest(MaasgroepTestFixture fixture)
		{
			Fixture = fixture;
			receiptStatusRepository = new Mock<IReceiptStatusRepository>();
			costCentreRepository = new Mock<ICostCentreRepository>();
			memberRepository = new Mock<IMemberRepository>();
		}

		public MaasgroepTestFixture Fixture { get; }

		private readonly Mock<IReceiptStatusRepository> receiptStatusRepository;
		private readonly Mock<ICostCentreRepository> costCentreRepository;
		private readonly Mock<IMemberRepository> memberRepository;

		#region GetById

		[Fact]
		public void GetById_With_Valid_Id_Returns_Data()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.GetModel(1);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id = 1;
			_ = result.As<ReceiptModel>().Note.Should().Be("Schroeven voor kapotte sloep");
			_ = result.As<ReceiptModel>().Amount.Should().Be(1.11M);
		}

		[Fact]
		public void GetById_With_InValid_Id_Returns_Null()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.GetModel(-1);

			_ = result.Should().BeNull();
		}

		#endregion

		#region GetModelInt

		[Fact]
		public void GetModelInt_With_Invalid_Id_Returns_Null()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.GetModel(-1);

			_ = result.Should().BeNull();
		}

		[Fact]
		public void GetModelInt_With_Valid_Id_Returns_ViewModel()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = sut.GetModel(1)!;

			var result = sut.GetModel(1);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptModel>().Note.Should().Be("Schroeven voor kapotte sloep");
			_ = result.As<ReceiptModel>().Amount.Should().Be(1.11M);
		}

		#endregion

		#region GetModelModel

		[Fact]
		public void GetModelModel_With_Valid_Id_AllPropsFilled_Ingediend_Returns_ViewModel()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = new ReceiptModel()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				Status = ReceiptStatus.Ingediend,
				StatusString = ReceiptStatus.Ingediend.ToString(),
				CostCentre = new CostCentreModel() { Id = -50, Name = "TestCostCentre" },
				IsEditable = true,
				IsApprovable = true,
				IsPayable = false,
				MemberCreated = new MemberModel() { Id = - 100, Name = "TestName" },
				DateTimeCreated = DateTime.Parse("12/30/2023 09:00:00"),
				DateTimeModified = DateTime.Parse("12/30/2023 09:10:00") //"20231230T09:10:00Z"
			};

			var costCentreToReturn = new CostCentreModel()
			{
				Id = -50,
				Name = "TestCostCentre"
			};

			var receiptStatusToReturn = ReceiptStatus.Ingediend;

			var memberToReturn = new MemberModel() { Id = -100, Name = "TestName" };

			var receiptToGet = new Receipt() {
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				MemberCreatedId = -100,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturn);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturn);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturn);

			var result = sut.GetModel(receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptModel>().Note.Should().Be(expected.Note);
			_ = result.As<ReceiptModel>().Amount.Should().Be(expected.Amount);
			_ = result.As<ReceiptModel>().Status.Should().Be(expected.Status);
			_ = result.As<ReceiptModel>().StatusString.Should().Be(expected.StatusString);
			_ = result.As<ReceiptModel>().CostCentre.Should().NotBeNull();
			_ = result.As<ReceiptModel>().CostCentre.Should().BeOfType<CostCentreModel>();
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Id.Should().Be(expected.CostCentre.Id);
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Name.Should().Be(expected.CostCentre.Name);
			_ = result.As<ReceiptModel>().IsPayable.Should().Be(expected.IsPayable);
			_ = result.As<ReceiptModel>().IsApprovable.Should().Be(expected.IsApprovable);
			_ = result.As<ReceiptModel>().IsEditable.Should().Be(expected.IsEditable);
			_ = result.As<ReceiptModel>().MemberCreated.Should().NotBeNull();
			_ = result.As<ReceiptModel>().MemberCreated.Should().BeOfType<MemberModel>();
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Id.Should().Be(expected.MemberCreated.Id);
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Name.Should().Be(expected.MemberCreated.Name);
			_ = result.As<ReceiptModel>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<ReceiptModel>().DateTimeModified.Should().Be(expected.DateTimeModified);
		}

		[Fact]
		public void GetModelModel_With_Valid_Id_AllPropsFilled_Onbekend_Returns_ViewModel()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = new ReceiptModel()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				Status = ReceiptStatus.Onbekend,
				StatusString = ReceiptStatus.Onbekend.ToString(),
				CostCentre = new CostCentreModel() { Id = -50, Name = "TestCostCentre" },
				IsEditable = true,
				IsApprovable = false,
				IsPayable = false,
				MemberCreated = new MemberModel() { Id = -100, Name = "TestName" },
				DateTimeCreated = DateTime.Parse("12/30/2023 09:00:00"),
				DateTimeModified = DateTime.Parse("12/30/2023 09:10:00") //"20231230T09:10:00Z"
			};

			var costCentreToReturn = new CostCentreModel()
			{
				Id = -50,
				Name = "TestCostCentre"
			};

			var receiptStatusToReturn = ReceiptStatus.Onbekend;

			var memberToReturn = new MemberModel() { Id = -100, Name = "TestName" };

			var receiptToGet = new Receipt()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				MemberCreatedId = -100,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturn);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturn);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturn);

			var result = sut.GetModel(receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptModel>().Note.Should().Be(expected.Note);
			_ = result.As<ReceiptModel>().Amount.Should().Be(expected.Amount);
			_ = result.As<ReceiptModel>().Status.Should().Be(expected.Status);
			_ = result.As<ReceiptModel>().StatusString.Should().Be(expected.StatusString);
			_ = result.As<ReceiptModel>().CostCentre.Should().NotBeNull();
			_ = result.As<ReceiptModel>().CostCentre.Should().BeOfType<CostCentreModel>();
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Id.Should().Be(expected.CostCentre.Id);
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Name.Should().Be(expected.CostCentre.Name);
			_ = result.As<ReceiptModel>().IsPayable.Should().Be(expected.IsPayable);
			_ = result.As<ReceiptModel>().IsApprovable.Should().Be(expected.IsApprovable);
			_ = result.As<ReceiptModel>().IsEditable.Should().Be(expected.IsEditable);
			_ = result.As<ReceiptModel>().MemberCreated.Should().NotBeNull();
			_ = result.As<ReceiptModel>().MemberCreated.Should().BeOfType<MemberModel>();
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Id.Should().Be(expected.MemberCreated.Id);
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Name.Should().Be(expected.MemberCreated.Name);
			_ = result.As<ReceiptModel>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<ReceiptModel>().DateTimeModified.Should().Be(expected.DateTimeModified);
		}

		[Fact]
		public void GetModelModel_With_Valid_Id_AllPropsFilled_Concept_Returns_ViewModel()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = new ReceiptModel()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				Status = ReceiptStatus.Concept,
				StatusString = ReceiptStatus.Concept.ToString(),
				CostCentre = new CostCentreModel() { Id = -50, Name = "TestCostCentre" },
				IsEditable = true,
				IsApprovable = false,
				IsPayable = false,
				MemberCreated = new MemberModel() { Id = -100, Name = "TestName" },
				DateTimeCreated = DateTime.Parse("12/30/2023 09:00:00"),
				DateTimeModified = DateTime.Parse("12/30/2023 09:10:00") //"20231230T09:10:00Z"
			};

			var costCentreToReturn = new CostCentreModel()
			{
				Id = -50,
				Name = "TestCostCentre"
			};

			var receiptStatusToReturn = ReceiptStatus.Concept;

			var memberToReturn = new MemberModel() { Id = -100, Name = "TestName" };

			var receiptToGet = new Receipt()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				MemberCreatedId = -100,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturn);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturn);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturn);

			var result = sut.GetModel(receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptModel>().Note.Should().Be(expected.Note);
			_ = result.As<ReceiptModel>().Amount.Should().Be(expected.Amount);
			_ = result.As<ReceiptModel>().Status.Should().Be(expected.Status);
			_ = result.As<ReceiptModel>().StatusString.Should().Be(expected.StatusString);
			_ = result.As<ReceiptModel>().CostCentre.Should().NotBeNull();
			_ = result.As<ReceiptModel>().CostCentre.Should().BeOfType<CostCentreModel>();
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Id.Should().Be(expected.CostCentre.Id);
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Name.Should().Be(expected.CostCentre.Name);
			_ = result.As<ReceiptModel>().IsPayable.Should().Be(expected.IsPayable);
			_ = result.As<ReceiptModel>().IsApprovable.Should().Be(expected.IsApprovable);
			_ = result.As<ReceiptModel>().IsEditable.Should().Be(expected.IsEditable);
			_ = result.As<ReceiptModel>().MemberCreated.Should().NotBeNull();
			_ = result.As<ReceiptModel>().MemberCreated.Should().BeOfType<MemberModel>();
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Id.Should().Be(expected.MemberCreated.Id);
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Name.Should().Be(expected.MemberCreated.Name);
			_ = result.As<ReceiptModel>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<ReceiptModel>().DateTimeModified.Should().Be(expected.DateTimeModified);
		}

		[Fact]
		public void GetModelModel_With_Valid_Id_MemberNull_Ingediend_Returns_ViewModel()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = new ReceiptModel()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				Status = ReceiptStatus.Ingediend,
				StatusString = ReceiptStatus.Ingediend.ToString(),
				CostCentre = new CostCentreModel() { Id = -50, Name = "TestCostCentre" },
				IsEditable = true,
				IsApprovable = true,
				IsPayable = false,
				MemberCreated = null,
				DateTimeCreated = DateTime.Parse("12/30/2023 09:00:00"),
				DateTimeModified = DateTime.Parse("12/30/2023 09:10:00") //"20231230T09:10:00Z"
			};

			var costCentreToReturn = new CostCentreModel()
			{
				Id = -50,
				Name = "TestCostCentre"
			};

			var receiptStatusToReturn = ReceiptStatus.Ingediend;

			MemberModel memberToReturn = null;

			var receiptToGet = new Receipt()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturn);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturn);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturn);

			var result = sut.GetModel(receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptModel>().Note.Should().Be(expected.Note);
			_ = result.As<ReceiptModel>().Amount.Should().Be(expected.Amount);
			_ = result.As<ReceiptModel>().Status.Should().Be(expected.Status);
			_ = result.As<ReceiptModel>().StatusString.Should().Be(expected.StatusString);
			_ = result.As<ReceiptModel>().CostCentre.Should().NotBeNull();
			_ = result.As<ReceiptModel>().CostCentre.Should().BeOfType<CostCentreModel>();
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Id.Should().Be(expected.CostCentre.Id);
			_ = result.As<ReceiptModel>().CostCentre.As<CostCentreModel>().Name.Should().Be(expected.CostCentre.Name);
			_ = result.As<ReceiptModel>().IsPayable.Should().Be(expected.IsPayable);
			_ = result.As<ReceiptModel>().IsApprovable.Should().Be(expected.IsApprovable);
			_ = result.As<ReceiptModel>().IsEditable.Should().Be(expected.IsEditable);
			_ = result.As<ReceiptModel>().MemberCreated.Should().BeNull();
			_ = result.As<ReceiptModel>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<ReceiptModel>().DateTimeModified.Should().Be(expected.DateTimeModified);
		}

		[Fact]
		public void GetModelModel_With_Valid_Id_CostCentreNull_Ingediend_Returns_ViewModel()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = new ReceiptModel()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				Status = ReceiptStatus.Ingediend,
				StatusString = ReceiptStatus.Ingediend.ToString(),
				CostCentre = null,
				IsEditable = true,
				IsApprovable = true,
				IsPayable = false,
				MemberCreated = new MemberModel() { Id = -100, Name = "TestName" },
				DateTimeCreated = DateTime.Parse("12/30/2023 09:00:00"),
				DateTimeModified = DateTime.Parse("12/30/2023 09:10:00") //"20231230T09:10:00Z"
			};

			CostCentreModel costCentreToReturn = null;

			var receiptStatusToReturn = ReceiptStatus.Ingediend;

			var memberToReturn = new MemberModel() { Id = -100, Name = "TestName" };

			var receiptToGet = new Receipt()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				MemberCreatedId = -100,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturn);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturn);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturn);

			var result = sut.GetModel(receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptModel>().Note.Should().Be(expected.Note);
			_ = result.As<ReceiptModel>().Amount.Should().Be(expected.Amount);
			_ = result.As<ReceiptModel>().Status.Should().Be(expected.Status);
			_ = result.As<ReceiptModel>().StatusString.Should().Be(expected.StatusString);
			_ = result.As<ReceiptModel>().CostCentre.Should().BeNull();
			_ = result.As<ReceiptModel>().IsPayable.Should().Be(expected.IsPayable);
			_ = result.As<ReceiptModel>().IsApprovable.Should().Be(expected.IsApprovable);
			_ = result.As<ReceiptModel>().IsEditable.Should().Be(expected.IsEditable);
			_ = result.As<ReceiptModel>().MemberCreated.Should().NotBeNull();
			_ = result.As<ReceiptModel>().MemberCreated.Should().BeOfType<MemberModel>();
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Id.Should().Be(expected.MemberCreated.Id);
			_ = result.As<ReceiptModel>().MemberCreated.As<MemberModel>().Name.Should().Be(expected.MemberCreated.Name);
			_ = result.As<ReceiptModel>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<ReceiptModel>().DateTimeModified.Should().Be(expected.DateTimeModified);
		}

		[Fact]
		public void GetModelModel_With_NullModel_Returns_ViewModel()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = new ReceiptModel()
			{
				Id = 0,
				Note = "TestReceipt",
				Amount = null,
				Status = ReceiptStatus.Onbekend,
				StatusString = ReceiptStatus.Onbekend.ToString(),
				CostCentre = null,
				IsEditable = true,
				IsApprovable = false,
				IsPayable = false,
				MemberCreated = null,
				DateTimeCreated = DateTime.Parse("01/01/0001 00:00:00"),
				DateTimeModified = null
			};

			CostCentreModel costCentreToReturn = null;

			var receiptStatusToReturn = ReceiptStatus.Onbekend;

			MemberModel memberToReturn = null;

			var receiptToGet = new Receipt();

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturn);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturn);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturn);

			var result = sut.GetModel(receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptModel>();
			_ = result.As<ReceiptModel>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptModel>().Note.Should().BeNull();
			_ = result.As<ReceiptModel>().Amount.Should().BeNull();
			_ = result.As<ReceiptModel>().Status.Should().Be(expected.Status);
			_ = result.As<ReceiptModel>().StatusString.Should().Be(expected.StatusString);
			_ = result.As<ReceiptModel>().CostCentre.Should().BeNull();
			_ = result.As<ReceiptModel>().IsPayable.Should().Be(expected.IsPayable);
			_ = result.As<ReceiptModel>().IsApprovable.Should().Be(expected.IsApprovable);
			_ = result.As<ReceiptModel>().IsEditable.Should().Be(expected.IsEditable);
			_ = result.As<ReceiptModel>().MemberCreated.Should().BeNull();
			_ = result.As<ReceiptModel>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<ReceiptModel>().DateTimeModified.Should().BeNull();
		}

		#endregion

		#region ListAllReadRepository

		[Fact]
		public void ListAll_GreatLimitThanImplementationIsPossible_NoHistoryResult3()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListAll(0, 5000, false);

			_ = result.Should().NotBeNull();
			result.Count().Should().Be(3);
		}

		[Fact]
		public void ListAll_GreatLimitThanImplementationIsPossible_WithHistoryResult4()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListAll(0, 5000, true);

			_ = result.Should().NotBeNull();
			result.Count().Should().Be(4);
		}

		#endregion

		#region Exists

		[Fact]
		public void Exists_WithValidId_ReturnsTrue()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.Exists(1L);

			_ = result.Should().Be(true);
		}

		[Fact]
		public void Exists_WithInvalidId_ReturnsFalse()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.Exists(343499);

			_ = result.Should().Be(false);
		}

		#endregion

		#region GetRecord

		[Fact]
		public void GetRecord_WithDefaultModels_ReturnsValue()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			ReceiptData receiptDataToGet = new ReceiptData();
			Receipt receiptToGet = new Receipt();

			var expected = new Receipt()
			{
				Id = 0,
				Amount = null,
				CostCentre = null,
				CostCentreId = null,
				DateTimeCreated = new DateTime(),
				DateTimeDeleted = null,
				DateTimeModified = null,
				Location = null,
				MemberCreated = null,
				MemberCreatedId = null,
				MemberDeleted = null,
				MemberDeletedId = null,
				MemberModified = null,
				MemberModifiedId = null,
				Note = null,
				Photos = null,
				ReceiptApprovals = null,
				ReceiptStatus = ReceiptStatus.Concept.ToString()
			};

			var result = sut.GetRecord(receiptDataToGet, receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<Receipt>();
			_ = result.As<Receipt>().Id.Should().Be(expected.Id);
			_ = result.As<Receipt>().Amount.Should().Be(expected.Amount);
			_ = result.As<Receipt>().CostCentre.Should().Be(expected.CostCentre);
			_ = result.As<Receipt>().CostCentreId.Should().Be(expected.CostCentreId);
			_ = result.As<Receipt>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<Receipt>().DateTimeDeleted.Should().Be(expected.DateTimeDeleted);
			_ = result.As<Receipt>().DateTimeModified.Should().Be(expected.DateTimeModified);
			_ = result.As<Receipt>().Location.Should().Be(expected.Location);
			_ = result.As<Receipt>().MemberCreated.Should().Be(expected.MemberCreated);
			_ = result.As<Receipt>().MemberCreatedId.Should().Be(expected.MemberCreatedId);
			_ = result.As<Receipt>().MemberDeleted.Should().Be(expected.MemberDeleted);
			_ = result.As<Receipt>().MemberDeletedId.Should().Be(expected.MemberDeletedId);
			_ = result.As<Receipt>().MemberModified.Should().Be(expected.MemberModified);
			_ = result.As<Receipt>().MemberModifiedId.Should().Be(expected.MemberModifiedId);
			_ = result.As<Receipt>().Note.Should().Be(expected.Note);
			_ = result.As<Receipt>().Photos.Should().BeNull();
			_ = result.As<Receipt>().ReceiptApprovals.Should().BeNull();
			_ = result.As<Receipt>().ReceiptStatus.Should().Be(expected.ReceiptStatus);
		}

		[Fact]
		public void GetRecord_WithDefaultModelsNotEditable_ReturnsNull()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			// Setting up data for GetModel(T) - START

			var costCentreToReturnFirst = new CostCentreModel()
			{
				Id = -50,
				Name = "TestCostCentre"
			};

			var receiptStatusToReturnFirst = ReceiptStatus.Uitbetaald;

			var memberToReturnFirst = new MemberModel() { Id = -100, Name = "TestName" };

			var receiptToGetFirst = new Receipt()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				MemberCreatedId = -100,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturnFirst);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturnFirst);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturnFirst);

			// Setting up data for GetModel(T) - END

			ReceiptData receiptDataToGet = new ReceiptData();
			Receipt receiptToGet = new Receipt();

			var expected = new Receipt()
			{
				Id = 0,
				Amount = null,
				CostCentre = null,
				CostCentreId = null,
				DateTimeCreated = new DateTime(),
				DateTimeDeleted = null,
				DateTimeModified = null,
				Location = null,
				MemberCreated = null,
				MemberCreatedId = null,
				MemberDeleted = null,
				MemberDeletedId = null,
				MemberModified = null,
				MemberModifiedId = null,
				Note = null,
				Photos = null,
				ReceiptApprovals = null,
				ReceiptStatus = ReceiptStatus.Concept.ToString()
			};

			var result = sut.GetRecord(receiptDataToGet, receiptToGet);

			_ = result.Should().BeNull();
		}

		[Fact]
		public void GetRecord_WithMinimalData_StatusConcept_ReturnsIngediend()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			ReceiptData receiptDataToGet = new ReceiptData()
			{
				Amount = 99.99m,
				Note = "test",
				CostCentreId = 1
			};
			Receipt receiptToGet = new Receipt() { Id = 1 };

			var expected = new Receipt()
			{
				Id = 1,
				Amount = 99.99m,
				CostCentre = null,
				CostCentreId = 1,
				DateTimeCreated = new DateTime(),
				DateTimeDeleted = null,
				DateTimeModified = null,
				Location = null,
				MemberCreated = null,
				MemberCreatedId = null,
				MemberDeleted = null,
				MemberDeletedId = null,
				MemberModified = null,
				MemberModifiedId = null,
				Note = "test",
				Photos = null,
				ReceiptApprovals = null,
				ReceiptStatus = ReceiptStatus.Ingediend.ToString()
			};

			var result = sut.GetRecord(receiptDataToGet, receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<Receipt>();
			_ = result.As<Receipt>().Id.Should().Be(expected.Id);
			_ = result.As<Receipt>().Amount.Should().Be(expected.Amount);
			_ = result.As<Receipt>().CostCentre.Should().Be(expected.CostCentre);
			_ = result.As<Receipt>().CostCentreId.Should().Be(expected.CostCentreId);
			_ = result.As<Receipt>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<Receipt>().DateTimeDeleted.Should().Be(expected.DateTimeDeleted);
			_ = result.As<Receipt>().DateTimeModified.Should().Be(expected.DateTimeModified);
			_ = result.As<Receipt>().Location.Should().Be(expected.Location);
			_ = result.As<Receipt>().MemberCreated.Should().Be(expected.MemberCreated);
			_ = result.As<Receipt>().MemberCreatedId.Should().Be(expected.MemberCreatedId);
			_ = result.As<Receipt>().MemberDeleted.Should().Be(expected.MemberDeleted);
			_ = result.As<Receipt>().MemberDeletedId.Should().Be(expected.MemberDeletedId);
			_ = result.As<Receipt>().MemberModified.Should().Be(expected.MemberModified);
			_ = result.As<Receipt>().MemberModifiedId.Should().Be(expected.MemberModifiedId);
			_ = result.As<Receipt>().Note.Should().Be(expected.Note);
			_ = result.As<Receipt>().Photos.Should().BeNull();
			_ = result.As<Receipt>().ReceiptApprovals.Should().BeNull();
			_ = result.As<Receipt>().ReceiptStatus.Should().Be(expected.ReceiptStatus);
		}

		[Fact]
		public void GetRecord_WithDefaultModelsNoPhotos_StatusConcept_ReturnsConcept()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			ReceiptData receiptDataToGet = new ReceiptData()
			{
				Amount = 99.99m,
				Note = "test",
				CostCentreId = 1
			};
			Receipt receiptToGet = new Receipt() { Id = 2 };

			var expected = new Receipt()
			{
				Id = 2,
				Amount = 99.99m,
				CostCentre = null,
				CostCentreId = 1,
				DateTimeCreated = new DateTime(),
				DateTimeDeleted = null,
				DateTimeModified = null,
				Location = null,
				MemberCreated = null,
				MemberCreatedId = null,
				MemberDeleted = null,
				MemberDeletedId = null,
				MemberModified = null,
				MemberModifiedId = null,
				Note = "test",
				Photos = null,
				ReceiptApprovals = null,
				ReceiptStatus = ReceiptStatus.Concept.ToString()
			};

			var result = sut.GetRecord(receiptDataToGet, receiptToGet);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<Receipt>();
			_ = result.As<Receipt>().Id.Should().Be(expected.Id);
			_ = result.As<Receipt>().Amount.Should().Be(expected.Amount);
			_ = result.As<Receipt>().CostCentre.Should().Be(expected.CostCentre);
			_ = result.As<Receipt>().CostCentreId.Should().Be(expected.CostCentreId);
			_ = result.As<Receipt>().DateTimeCreated.Should().Be(expected.DateTimeCreated);
			_ = result.As<Receipt>().DateTimeDeleted.Should().Be(expected.DateTimeDeleted);
			_ = result.As<Receipt>().DateTimeModified.Should().Be(expected.DateTimeModified);
			_ = result.As<Receipt>().Location.Should().Be(expected.Location);
			_ = result.As<Receipt>().MemberCreated.Should().Be(expected.MemberCreated);
			_ = result.As<Receipt>().MemberCreatedId.Should().Be(expected.MemberCreatedId);
			_ = result.As<Receipt>().MemberDeleted.Should().Be(expected.MemberDeleted);
			_ = result.As<Receipt>().MemberDeletedId.Should().Be(expected.MemberDeletedId);
			_ = result.As<Receipt>().MemberModified.Should().Be(expected.MemberModified);
			_ = result.As<Receipt>().MemberModifiedId.Should().Be(expected.MemberModifiedId);
			_ = result.As<Receipt>().Note.Should().Be(expected.Note);
			_ = result.As<Receipt>().Photos.Should().BeNull();
			_ = result.As<Receipt>().ReceiptApprovals.Should().BeNull();
			_ = result.As<Receipt>().ReceiptStatus.Should().Be(expected.ReceiptStatus);
		}

		#endregion

		#region Create

		[Fact]
		public void Create_WithDefaultModels_ReturnsNull()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			ReceiptData receiptDataToGet = new ReceiptData();
			Receipt receiptToGet = new Receipt();

			// Setting up data for GetModel(T) - START

			var costCentreToReturnFirst = new CostCentreModel()
			{
				Id = -50,
				Name = "TestCostCentre"
			};

			var receiptStatusToReturnFirst = ReceiptStatus.Uitbetaald;

			var memberToReturnFirst = new MemberModel() { Id = -100, Name = "TestName" };

			var receiptToGetFirst = new Receipt()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				MemberCreatedId = -100,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturnFirst);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturnFirst);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturnFirst);

			// Setting up data for GetModel(T) - END

			var expected = new Receipt()
			{
				Id = 0,
				Amount = null,
				CostCentre = null,
				CostCentreId = null,
				DateTimeCreated = new DateTime(),
				DateTimeDeleted = null,
				DateTimeModified = null,
				Location = null,
				MemberCreated = null,
				MemberCreatedId = null,
				MemberDeleted = null,
				MemberDeletedId = null,
				MemberModified = null,
				MemberModifiedId = null,
				Note = null,
				Photos = null,
				ReceiptApprovals = null,
				ReceiptStatus = ReceiptStatus.Concept.ToString()
			};

			var result = sut.Create(receiptDataToGet, null);

			_ = result.Should().BeNull();
		}

	

		#endregion

		#region ListByMember

		[Fact]
		public void ListByMember_WithValidId_NoDeleted_OverLimit_ReturnsValue3()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var memberId = 1L;

			var result = sut.ListByMember(memberId, 0, 5000, false);

			_ = result.Should().NotBeNull();
			result.Count().Should().Be(3);
		}

		[Fact]
		public void ListByMember_WithInvalidId_NoDeleted_OverLimit_ReturnsValue0()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var memberId = 77785671L;

			var result = sut.ListByMember(memberId, 0, 5000, false);

			_ = result.Should().NotBeNull();
			result.Count().Should().Be(0);
		}

		[Fact]
		public void ListByMember_WithValidId_WithDeleted_OverLimit_ReturnsValue4()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var memberId = 1L;

			var result = sut.ListByMember(memberId, 0, 5000, true);

			_ = result.Should().NotBeNull();
			result.Count().Should().Be(4);
		}

		#endregion

		#region GetHistory

		[Fact]
		public void GetHistory_WithDefaultModels_ReturnsValue()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var expected = new ReceiptHistory()
			{
				Id = 0,
				ReceiptId = -1,
				Amount = 12.34m,
				Note = "test",
				Location = null,
				ReceiptStatus = ReceiptStatus.Onbekend.ToString(),
				CostCentreId = -1
			};

			var receiptToGetHistoryOf = new Receipt()
			{
				Id = -1,
				Amount = 12.34m,
				Note = "test",
				Location = null,
				ReceiptStatus = ReceiptStatus.Onbekend.ToString(),
				CostCentreId = -1
			};

			var result = sut.GetHistory(receiptToGetHistoryOf);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ReceiptHistory>();
			_ = result.As<ReceiptHistory>();
			_ = result.As<ReceiptHistory>().Id.Should().Be(expected.Id);
			_ = result.As<ReceiptHistory>().ReceiptId.Should().Be(expected.ReceiptId);
			_ = result.As<ReceiptHistory>().Amount.Should().Be(expected.Amount);
			_ = result.As<ReceiptHistory>().Note.Should().Be(expected.Note);
			_ = result.As<ReceiptHistory>().Location.Should().Be(expected.Location);
			_ = result.As<ReceiptHistory>().ReceiptStatus.Should().Be(expected.ReceiptStatus);
			_ = result.As<ReceiptHistory>().CostCentreId.Should().Be(expected.CostCentreId);
		}

		#endregion

		#region Update

		[Fact]
		public void Update_WithMinimalData_newRecordNull_ReturnsNull()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			ReceiptData receiptDataToGet = new ReceiptData()
			{
				Amount = 99.99m,
				Note = "test",
				CostCentreId = 1
			};
			Receipt receiptToGet = new Receipt() { Id = 1 };

			// Setting up data for GetModel(T) - START

			var costCentreToReturnFirst = new CostCentreModel()
			{
				Id = -50,
				Name = "TestCostCentre"
			};

			var receiptStatusToReturnFirst = ReceiptStatus.Uitbetaald;

			var memberToReturnFirst = new MemberModel() { Id = -100, Name = "TestName" };

			var receiptToGetFirst = new Receipt()
			{
				Id = -1,
				Note = "TestReceipt",
				Amount = 20.50m,
				CostCentreId = -50,
				MemberCreatedId = -100,
				DateTimeCreated = new DateTime(2023, 12, 30, 9, 0, 0),
				DateTimeModified = new DateTime(2023, 12, 30, 9, 10, 0),
			};

			receiptStatusRepository.Setup(r => r.GetModel(It.IsAny<string>())).Returns(receiptStatusToReturnFirst);
			costCentreRepository.Setup(c => c.GetModel(It.IsAny<long>())).Returns(costCentreToReturnFirst);
			memberRepository.Setup(m => m.GetModel(It.IsAny<long>())).Returns(memberToReturnFirst);

			// Setting up data for GetModel(T) - END

			var result = sut.Update(receiptToGetFirst, receiptDataToGet, null);

			_ = result.Should().BeNull();
			
		}

		#endregion

		#region ListByCostCentre

		[Fact]
		public void ListByCostCentre_With_InValid_Id_Returns_Data()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListByCostCentre(-5);

			_ = result.Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().Count().Should().Be(0);
		}

		[Fact]
		public void ListByCostCentre_With_Valid_Id_Returns_TwoRecords()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListByCostCentre(3);

			_ = result.Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().Count().Should().Be(2);
		}

		[Fact]
		public void ListByCostCentre_With_Valid_Id_AndHist_Returns_ThreeRecords()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListByCostCentre(3, includeDeleted: true);

			_ = result.Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().Count().Should().Be(3);
		}

		[Fact]
		public void ListByCostCentre_With_Valid_Id_AndHist_TakeOneOffsetOne_Returns_OneRecords()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListByCostCentre(3, 1, 1, false);

			_ = result.Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().Count().Should().Be(1);
			_ = result.As<IEnumerable<ReceiptModel>>().ElementAt(0).Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().ElementAt(0).Amount.Should().Be(3.33m);
			_ = result.As<IEnumerable<ReceiptModel>>().ElementAt(0).Id.Should().Be(3);
			_ = result.As<IEnumerable<ReceiptModel>>().ElementAt(0).Note.Should().Be("Handboek Woudlopers");
		}

		#endregion

		#region ListPayable

		[Fact]
		public void ListPayable_WithDefaultValues_ReturnsResult()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListPayable();

			_ = result.Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().Count().Should().Be(1);
			_ = result.As<IEnumerable<ReceiptModel>>().ElementAt(0).Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().ElementAt(0).Id.Should().Be(2);
			_ = result.As<IEnumerable<ReceiptModel>>().ElementAt(0).Note.Should().Be("Knakworstjes in de bonus");
		}

		[Fact]
		public void ListPayable_WithOffsetValues_ReturnsResult()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			var result = sut.ListPayable(1);

			_ = result.Should().NotBeNull();
			_ = result.As<IEnumerable<ReceiptModel>>().Count().Should().Be(0);
		}

		#endregion
	}
}
