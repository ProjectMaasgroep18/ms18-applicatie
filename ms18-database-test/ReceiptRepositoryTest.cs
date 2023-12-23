using FluentAssertions;
using Maasgroep.Database.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Moq;
using Maasgroep.Database.Interfaces;

namespace Maasgroep.Database.Test
{
	// Documentatie
	// https://learn.microsoft.com/en-us/ef/core/testing/
	// https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-7.0

	public class ReceiptRepositoryTest : IClassFixture<MaasgroepTestFixture>
	{
		public ReceiptRepositoryTest(MaasgroepTestFixture fixture) => Fixture = fixture;

		public MaasgroepTestFixture Fixture { get; }



		#region GetReceipt

		[Fact]
		public void GetReceipt_With_Valid_Id_Returns_Data()
		{
			using var context = Fixture.CreateContext();

			var receiptStatusRepository = new Mock<IReceiptStatusRepository>();
			var costCentreRepository = new Mock<ICostCentreRepository>();
			var memberRepository = new Mock<IMemberRepository>();

			var sut = new ReceiptRepository(context
				, receiptStatusRepository.Object
				, costCentreRepository.Object
				, memberRepository.Object);

			//var receipt = sut.GetReceipt(1);

			//_ = receipt.Note.Should().Be("Schroeven voor kapotte sloep");
			//_ = receipt.Amount.Should().Be(1.11M);
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
