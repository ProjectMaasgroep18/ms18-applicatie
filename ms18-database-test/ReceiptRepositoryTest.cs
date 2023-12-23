using FluentAssertions;
using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Moq;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.DataModels.Receipts;

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

		/*
		GetModel (int)		ReadRepository				Done
		GetModel (model)	ReadRepository
		Exists				ReadRepository				Done
		GetById				ReadRepository				Done
		ListAll				ReadRepository				Done
		GetRecord			WriteableRepository
		ListByMember		WriteableRepository			Done
		Create				WriteableRepository
		GetSaveAction		WriteableRepository
		GetAfterSaveAction	WriteableRepository
		Delete				DeletableRepository
		GetList				DeletableRepository
		GetList (bool)		DeletableRepository
		ListAll				DeletableRepository
		ListByMember		DeletableRepository
		GetHistory			EditableRepository
		Update				EditableRepository
		ListByCostCentre	ReceiptRepository
		ListPayable			ReceiptRepository
		*/

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

		//TODO: KH deze region later, is veel werk
		#region GetModelModel

		[Fact]
		public void GetModelModel_With_Valid_Id_Returns_ViewModel()
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

		#region ListAll

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

		//TODO: KH deze region later, is veel werk
		#region GetRecord

		[Fact]
		public void GetRecord_WithValidId_ReturnsValue()
		{
			using var context = Fixture.CreateContext();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			ReceiptData receiptToGet = new ReceiptData();

			var result = sut.GetRecord(receiptToGet);

			_ = result.Should().NotBeNull();
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







		//[Fact]
		//public void GetReceipt_With_InValid_Id_ThrowsError()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);

		//	var expected = new InvalidOperationException();
		//	object actual = new object();

		//	try
		//	{
		//		actual = sut.GetReceipt(-1);
		//	}
		//	catch (Exception ex)
		//	{
		//		actual = ex;
		//	}

		//	actual.Should().BeOfType<InvalidOperationException>();
		//}

		//#endregion

		//#region GetReceipts

		//[Fact]
		//public void GetReceipts_With_Valid_Range_Returns_CountGreaterThanZero()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);

		//	var actual = sut.GetReceipts(0, 0);

		//	_ = actual.Should().BeOfType<List<ReceiptModel>>();
		//	_ = actual.Count().Should().BeGreaterThan(0);

		//}

		//#endregion

		//#region GetReceiptsByMember

		//[Fact]
		//public void GetReceiptsByMember_With_Valid_Range_Returns_CountGreaterThanZero()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);

		//	var actual = sut.GetReceiptsByMember(1, 0, 0);

		//	_ = actual.Should().BeOfType<List<ReceiptModel>>();
		//	_ = actual.Count().Should().BeGreaterThan(0);
		//}

		//[Fact]
		//public void GetReceiptsByMember_With_InvalidMember_Returns_ZeroList()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);

		//	var actual = sut.GetReceiptsByMember(0, 0, 0);

		//	_ = actual.Should().BeOfType<List<ReceiptModel>>();
		//	_ = actual.Count().Should().Be(0);
		//}

		//#endregion

		//#region Add

		//[Fact]
		//public void Add_With_CostCentreNull_ThrowsError()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);
		//	var receiptModelMock = new Mock<ReceiptModelCreateDb>().Object;
		//	receiptModelMock.ReceiptModel = new Mock<ReceiptModelCreate>().Object;
		//	receiptModelMock.ReceiptModel.CostCentre = "pietje";
		//	object actual = new object();

		//	//kh, door een lege costcentre komt je niet eens door de where van linq (regel 97)) (ergo mock)
		//	try
		//	{
		//		actual = sut.Add(receiptModelMock);
		//	}
		//	catch (Exception ex)
		//	{
		//		actual = ex;
		//	}

		//	_ = actual.Should().BeOfType<Exception>();
		//	_ = actual.As<Exception>().Message.Should().Be("kapot!");
		//}

		//[Fact]
		//public void Add_With_ValidObject_GivesCorrectId()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);

		//	var modelToAdd = new ReceiptModelCreateDb()
		//	{
		//		Member = new MemberModel()
		//		{
		//			Id = 1,
		//			Name = "kandit?",
		//			Permissions = new List<PermissionModel>()
		//		},
		//		ReceiptModel = new ReceiptModelCreate()
		//		{
		//			Amount = 44.44M,
		//			CostCentre = "Bestuur Maasgroep",
		//			Note = "lekkere test dit",
		//			Photos = new List<PhotoModelCreate>()
		//					{
		//						new PhotoModelCreate()
		//						{
		//							FileExtension = "nb",
		//							FileName = "Foto van gras",
		//							Base64Image = "doemaargewoonatteskt"
		//						}
		//					}
		//		}
		//	};

		//	var expected = context.Receipt.Max(r => r.Id) + 1;
		//	object actual = new object();

		//	//kh, door een lege costcentre komt je niet eens door de where van linq (regel 97)) (ergo mock)
		//	try
		//	{
		//		actual = sut.Add(modelToAdd);
		//	}
		//	catch (Exception ex)
		//	{
		//		actual = ex;
		//	}

		//	_ = actual.Should().BeOfType<long>();
		//	_ = actual.Should().Be(expected);
		//}

		//#endregion

		//#region Modify
		//[Fact]
		//public void Modify_With_ReceiptNull_ThrowsError()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);
		//	var receiptModelMock = new Mock<ReceiptModelUpdateDb>().Object;
		//	receiptModelMock.ReceiptModel = new Mock<ReceiptModel>().Object;
		//	receiptModelMock.ReceiptModel.Id = -1;
		//	object actual = new object();

		//	//kh, door een lege costcentre komt je niet eens door de where van linq (regel 97)) (ergo mock)
		//	try
		//	{
		//		actual = sut.Modify(receiptModelMock);
		//	}
		//	catch (Exception ex)
		//	{
		//		actual = ex;
		//	}

		//	_ = actual.Should().BeOfType<Exception>();
		//	_ = actual.As<Exception>().Message.Should().Be("kapot!");
		//}

		//[Fact]
		//public void Modify_With_ValidObject_ReturnsTrue()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);

		//	var modelToAdd = new ReceiptModelCreateDb()
		//	{
		//		Member = new MemberModel()
		//		{
		//			Id = 1,
		//			Name = "kandit?",
		//			Permissions = new List<PermissionModel>()
		//		},
		//		ReceiptModel = new ReceiptModelCreate()
		//		{
		//			Amount = 44.44M,
		//			CostCentre = "Bestuur Maasgroep",
		//			Note = "lekkere test dit",
		//			Photos = new List<PhotoModelCreate>()
		//					{
		//						new PhotoModelCreate()
		//						{
		//							FileExtension = "nb",
		//							FileName = "Foto van gras",
		//							Base64Image = "doemaargewoonatteskt"
		//						}
		//					}
		//		}
		//	};

		//	var modelToUpdate = new ReceiptModelUpdateDb()
		//	{
		//		Member = new MemberModel()
		//		{
		//			Id = 2,
		//			Name = "kandit?",
		//			Permissions = new List<PermissionModel>()
		//		},
		//		ReceiptModel = new ReceiptModel()
		//		{
		//			Id = 4,
		//			Amount = 44.55M,
		//			Note = "Dees is de modify van ditte" + DateTime.Now.ToString(),
		//			CostCentre = new CostCentreModel()
		//			{
		//				Name = "Bestuur Maasgroep",
		//				Id = 1
		//			},

		//			Photos = new List<PhotoModel>()
		//					{
		//						new PhotoModel()
		//						{
		//							Id = 1,
		//							fileExtension = "nb",
		//							fileName = "Foto van gras",
		//							Base64Image = "gaatdiefotoookmee?"
		//						}
		//					}
		//		}
		//	};

		//	var expected = true;
		//	object actual = new object();

		//	//kh, door een lege costcentre komt je niet eens door de where van linq (regel 97)) (ergo mock)
		//	try
		//	{
		//		var id = sut.Add(modelToAdd);
		//		modelToUpdate.ReceiptModel.Id = id;
		//		actual = sut.Modify(modelToUpdate);
		//	}
		//	catch (Exception ex)
		//	{
		//		actual = ex;
		//	}

		//	_ = actual.Should().BeOfType<bool>();
		//	_ = actual.Should().Be(expected);
		//}
		//#endregion

		//#region Delete

		//[Fact]
		//public void Delete_With_ReceiptNull_ThrowsError()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);
		//	var receiptModelMock = new Mock<ReceiptModelDeleteDb>().Object;
		//	receiptModelMock.Receipt = new Mock<ReceiptModel>().Object;
		//	receiptModelMock.Receipt.Id = -1;
		//	object actual = new object();

		//	//kh, door een lege costcentre komt je niet eens door de where van linq (regel 97)) (ergo mock)
		//	try
		//	{
		//		actual = sut.Delete(receiptModelMock);
		//	}
		//	catch (Exception ex)
		//	{
		//		actual = ex;
		//	}

		//	_ = actual.Should().BeOfType<bool>();
		//	_ = actual.Should().Be(false);
		//}

		//[Fact]
		//public void Delete_With_ValidObject_ReturnsTrue()
		//{
		//	using var context = Fixture.CreateContext();
		//	var sut = new ReceiptRepository(context);

		//	var modelToAdd = new ReceiptModelCreateDb()
		//	{
		//		Member = new MemberModel()
		//		{
		//			Id = 1,
		//			Name = "kandit?",
		//			Permissions = new List<PermissionModel>()
		//		},
		//		ReceiptModel = new ReceiptModelCreate()
		//		{
		//			Amount = 44.44M,
		//			CostCentre = "Bestuur Maasgroep",
		//			Note = "lekkere test dit",
		//			Photos = new List<PhotoModelCreate>()
		//					{
		//						new PhotoModelCreate()
		//						{
		//							FileExtension = "nb",
		//							FileName = "Foto van gras",
		//							Base64Image = "doemaargewoonatteskt"
		//						}
		//					}
		//		}
		//	};

		//	var modelToDelete = new ReceiptModelDeleteDb()
		//	{
		//		Member = new MemberModel()
		//		{
		//			Id = 2,
		//			Name = "kandit?",
		//			Permissions = new List<PermissionModel>()
		//		},
		//		Receipt = new ReceiptModel()
		//		{
		//			Id = 4,
		//			Amount = 44.55M,
		//			Note = "Dees is de modify van ditte" + DateTime.Now.ToString(),
		//			CostCentre = new CostCentreModel()
		//			{
		//				Name = "Bestuur Maasgroep",
		//				Id = 1
		//			},

		//			Photos = new List<PhotoModel>()
		//					{
		//						new PhotoModel()
		//						{
		//							Id = 1,
		//							fileExtension = "nb",
		//							fileName = "Foto van gras",
		//							Base64Image = "gaatdiefotoookmee?"
		//						}
		//					}
		//		}
		//	};

		//	var expected = true;
		//	object actual = new object();

		//	//kh, door een lege costcentre komt je niet eens door de where van linq (regel 97)) (ergo mock)
		//	try
		//	{
		//		var id = sut.Add(modelToAdd);
		//		modelToDelete.Receipt.Id = id;
		//		actual = sut.Delete(modelToDelete);
		//	}
		//	catch (Exception ex)
		//	{
		//		actual = ex;
		//	}

		//	_ = actual.Should().BeOfType<bool>();
		//	_ = actual.Should().Be(expected);
		//}
		//#endregion
	}
}
