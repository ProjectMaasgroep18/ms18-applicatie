using Moq;
using FluentAssertions;
using Maasgroep.Controllers.Api;
using Maasgroep.Interfaces;
using Maasgroep.Database.Interfaces;
using Maasgroep.SharedKernel.DataModels.Receipts;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Maasgroep.Exceptions;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.Database.Receipts;

namespace ms18_applicatie_test
{
	public class ReceiptControllerTests
	{
		Mock<IReceiptStatusRepository> receiptStatusRepository;
		Mock<ICostCentreRepository> costCentreRepository;
		Mock<IMemberRepository> memberRepository;
		Mock<IReceiptRepository> receiptRepository;
		Mock<IReceiptPhotoRepository> receiptPhotoRepository;
		Mock<IReceiptApprovalRepository> receiptApprovalRepository;
		Mock<IMaasgroepAuthenticationService> maasgroepAuthenticationService;

		public ReceiptControllerTests()
		{
			receiptStatusRepository = new Mock<IReceiptStatusRepository>();
			costCentreRepository = new Mock<ICostCentreRepository>();
			memberRepository = new Mock<IMemberRepository>();
			receiptRepository = new Mock<IReceiptRepository>();
			receiptPhotoRepository = new Mock<IReceiptPhotoRepository>();
			receiptApprovalRepository = new Mock<IReceiptApprovalRepository>();
			maasgroepAuthenticationService = new Mock<IMaasgroepAuthenticationService>();
		}
		

		#region RepositoryGet

		[Fact]
		public void RepositoryGet_ValidMemberPermission_GivesOkResult()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			var result = sut.RepositoryGet();

			_ = result.Should().NotBeNull();
		}

		[Fact]
		public void RepositoryGet_InValidMemberPermission_ThrowsMaasgroepForbiddenError()
		{
			SetAuthentication_PermissionWrong();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryGet();
			}
			catch (MaasgroepForbidden forbidden)
			{
				result = forbidden;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be("Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void RepositoryGet_NotLoggedIn_ThrowsMaasgroepUnauthorizedException()
		{
			SetAuthentication_Unauthenticated();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryGet();
			}
			catch (MaasgroepUnauthorized forbidden)
			{
				result = forbidden;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepUnauthorized>();
			_ = result.As<MaasgroepUnauthorized>().Message.Should().Be("Je bent niet ingelogd");
		}

		#endregion

		#region RepositoryGetById

		[Fact]
		public void RepositoryGetById_ValidMemberPermission_ExistingReceipt_GivesOkResult()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			var receipt = new Receipt()
			{ 
				Amount = 1,
				Id = 1,
				CostCentreId = 1,
				DateTimeCreated = DateTime.Now,
				MemberCreatedId = 1,
				Note = "empty note, or is it"
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			var result = sut.RepositoryGetById(1);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
		}

		[Fact]
		public void RepositoryGetById_InvalidMemberPermission_ExistingReceipt_ThrowsMaasgroepForbiddenError()
		{
			SetAuthentication_PermissionWrong();

			var receipt = new Receipt()
			{
				Amount = 1,
				Id = 1,
				CostCentreId = 1,
				DateTimeCreated = DateTime.Now,
				MemberCreatedId = 3,
				Note = "empty note, or is it"
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryGetById(1);
			}
			catch (MaasgroepForbidden ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be("Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void RepositoryGetById_InvalidMemberPermission_OwnsReceipt_GivesOkResult()
		{
			SetAuthentication_PermissionWrong();

			var receipt = new Receipt()
			{
				Amount = 1,
				Id = 1,
				CostCentreId = 1,
				DateTimeCreated = DateTime.Now,
				MemberCreatedId = 1,
				Note = "MemberCreated = Member Authenticated"
			};

			var receiptModel = new ReceiptModel()
			{
				Amount = 1,
				Id = 1,
				DateTimeCreated = DateTime.Now,
				MemberCreated = null,
				CostCentre = null,
				Note = "Test geslaagd :)"
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receipt);
			receiptRepository.Setup(r => r.GetModel(receipt)).Returns(receiptModel);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryGetById(1);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().NotBeNull();
			_ = result.As<OkObjectResult>().Value.Should().BeOfType<ReceiptModel>();
			_ = result.As<OkObjectResult>().Value.As<ReceiptModel>().Note.Should().Be("Test geslaagd :)");
		}

		[Fact]
		public void RepositoryGetById_InvalidMemberPermission_NullReceipt_ThrowsMaasgroepForbiddenError()
		{
			SetAuthentication_PermissionWrong();

			Receipt receipt = null;

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryGetById(1);
			}
			catch (MaasgroepForbidden ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be("Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void RepositoryGetById_ValidMemberPermission_NullReceipt_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			Receipt receipt = null;

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryGetById(1);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void RepositoryGetById_NotLoggedIn_ExistingReceipt_ThrowsMaasgroepUnauthorizedException()
		{
			SetAuthentication_Unauthenticated();

			var receipt = new Receipt()
			{
				Amount = 1,
				Id = 1,
				CostCentreId = 1,
				DateTimeCreated = DateTime.Now,
				MemberCreatedId = 3,
				Note = "empty note, or is it"
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryGetById(1);
			}
			catch (MaasgroepUnauthorized ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepUnauthorized>();
			_ = result.As<MaasgroepUnauthorized>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepUnauthorized>().Message.Should().Be("Je bent niet ingelogd");
		}

		#endregion

		#region RepositoryCreate

		[Fact]
		public void RepositoryCreate_NotLoggedIn_ThrowsMaasgroepUnauthorizedException()
		{
			SetAuthentication_Unauthenticated();

			ReceiptData receipt = null;

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryCreate(receipt);
			}
			catch (MaasgroepUnauthorized ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepUnauthorized>();
			_ = result.As<MaasgroepUnauthorized>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepUnauthorized>().Message.Should().Be("Je bent niet ingelogd");
		}

		[Fact]
		public void RepositoryCreate_InvalidMemberPermission_ThrowsMaasgroepForbiddenException()
		{
			SetAuthentication_PermissionWrong();

			ReceiptData receipt = null;

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryCreate(receipt);
			}
			catch (MaasgroepForbidden ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be("Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void RepositoryCreate_ValidMemberPermission_NoData_ThrowsMaasgroepBadRequestException()
		{
			SetAuthentication_PermissionCorrect("receipt");

			ReceiptData receipt = null;

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryCreate(receipt);
			}
			catch (MaasgroepBadRequest ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepBadRequest>();
			_ = result.As<MaasgroepBadRequest>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepBadRequest>().Message.Should().Be("Declaratie kon niet worden aangemaakt");
		}

		[Fact]
		public void RepositoryCreate_ValidMemberPermission_ValidData_ThrowsMaasgroepBadRequestException()
		{
			SetAuthentication_PermissionCorrect("receipt");

			Receipt returnedData = new()
			{
				Amount = 1,
				Id = 20
			};

			ReceiptModel returnedView = new()
			{ 
				Amount = 1,
				Id = 20,
				Note = "Test geslaagd"
			};

			receiptRepository.Setup(r => r.Create(It.IsAny<ReceiptData>(), 1)).Returns(returnedData);
			receiptRepository.Setup(r => r.GetModel(It.IsAny<Receipt>())).Returns(returnedView);

			ReceiptData receipt = new()
			{
				Amount = 1,
				CostCentreId = 1
			};

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryCreate(receipt);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			// Routedata kan niet benaderd
			_ = result.Should().BeOfType<CreatedResult>();
			_ = result.As<CreatedResult>().Value.Should().NotBeNull();
			_ = result.As<CreatedResult>().Value.Should().Be("Test geslaagd");
		}

		#endregion

		#region RepositoryDelete

		[Fact]
		public void RepositoryDelete_NotLoggedIn__NullRecord_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryDelete(0);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void RepositoryDelete_NotLoggedIn_ValidData_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryDelete(0);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void RepositoryDelete_InValidPermission_ValidData_ThrowsMaasgroepForbiddenException()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryDelete(0);
			}
			catch (MaasgroepForbidden ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be($"Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void RepositoryDelete_ValidPermission_ValidData_RepoFalse_ThrowsMaasgroepBadRequestException()
		{
			SetAuthentication_PermissionCorrect("admin");

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);
			receiptRepository.Setup(r => r.Delete(getByIdReceipt, It.IsAny<long>())).Returns(false);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryDelete(0);
			}
			catch (MaasgroepBadRequest ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepBadRequest>();
			_ = result.As<MaasgroepBadRequest>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepBadRequest>().Message.Should().Be($"Declaratie kon niet worden verwijderd");
		}

		[Fact]
		public void RepositoryDelete_ValidPermission_ValidData_RepoTrue_ReturnsNoContent()
		{
			SetAuthentication_PermissionCorrect("admin");

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);
			receiptRepository.Setup(r => r.Delete(getByIdReceipt, It.IsAny<long>())).Returns(true);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryDelete(0);
			}
			catch (MaasgroepBadRequest ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NoContentResult>();
			_ = result.As<NoContentResult>().StatusCode.Should().Be(204);
		}

		#endregion

		#region RepositoryUpdate

		[Fact]
		public void RepositoryUpdate_NotLoggedIn_NullData_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryUpdate(0, repoUpdateData);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void RepositoryUpdate_NotLoggedIn_ReceiptExist_ThrowsMaasgroepUnauthorizedException()
		{
			SetAuthentication_Unauthenticated();

			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), true)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryUpdate(0, repoUpdateData);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void RepositoryUpdate_LoggedIn_WrongPermission_ThrowsMaasgroepForbiddenException()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryUpdate(0, repoUpdateData);
			}
			catch (MaasgroepForbidden ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be($"Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void RepositoryUpdate_LoggedIn_CorrectPermission_DataNull_ThrowsMaasgroepBadRequestException()
		{
			SetAuthentication_PermissionCorrect("admin");

			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			Receipt repoDataTurn = null;

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);
			receiptRepository.Setup(r => r.Update(It.IsAny<Receipt>(), It.IsAny<ReceiptData>(), -1)).Returns(repoDataTurn);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryUpdate(0, repoUpdateData);
			}
			catch (MaasgroepBadRequest ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepBadRequest>();
			_ = result.As<MaasgroepBadRequest>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepBadRequest>().Message.Should().Be($"Declaratie kon niet worden opgeslagen");
		}

		[Fact]
		public void RepositoryUpdate_LoggedIn_CorrectPermission_DataCorrect_ReturnsNoContentResult()
		{
			SetAuthentication_PermissionCorrect("admin");

			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			Receipt repoDataTurn = new()
			{
				Note = "test"
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);
			receiptRepository.Setup(r => r.Update(It.IsAny<Receipt>(), It.IsAny<ReceiptData>(), It.IsAny<long>())).Returns(repoDataTurn);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryUpdate(0, repoUpdateData);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NoContentResult>();
			_ = result.As<NoContentResult>().StatusCode.Should().Be(204);
		}

		#endregion

		#region ReceiptAddPhoto

		[Fact]
		public void ReceiptAddPhoto_NotLoggedIn_NullData_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			ReceiptPhotoData addPhotoData = new()
			{
				Base64Image = "doet er niet toe",
				FileExtension = "kevin",
				FileName = "hoi!",
				ReceiptId = 1
			};

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptAddPhoto(0, addPhotoData);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptAddPhoto_NotLoggedIn_ReceiptExists_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			ReceiptPhotoData addPhotoData = new()
			{
				Base64Image = "doet er niet toe",
				FileExtension = "kevin",
				FileName = "hoi!",
				ReceiptId = 1
			};


			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptAddPhoto(0, addPhotoData);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptAddPhoto_LoggedIn_InvalidPermission_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_PermissionWrong();

			ReceiptPhotoData addPhotoData = new()
			{
				Base64Image = "doet er niet toe",
				FileExtension = "kevin",
				FileName = "hoi!",
				ReceiptId = 1
			};


			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptAddPhoto(0, addPhotoData);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptAddPhoto_LoggedIn_PermissionCorrect_ThrowsMaasgroepForbiddenException()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			ReceiptPhotoData addPhotoData = new()
			{
				Base64Image = "doet er niet toe",
				FileExtension = "kevin",
				FileName = "hoi!",
				ReceiptId = 1
			};


			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptAddPhoto(0, addPhotoData);
			}
			catch (MaasgroepForbidden ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be("Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void ReceiptAddPhoto_LoggedIn_PermissionCorrect_PhotoCreateNull_ThrowMaasgroepBadRequestException()
		{
			SetAuthentication_PermissionCorrect("admin");

			ReceiptPhotoData addPhotoData = new()
			{
				Base64Image = "doet er niet toe",
				FileExtension = "kevin",
				FileName = "hoi!",
				ReceiptId = 1
			};


			ReceiptData repoUpdateData = new()
			{
				Amount = 15,
				CostCentreId = 2,
				Note = "updated ding"
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);


			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptAddPhoto(0, addPhotoData);
			}
			catch (MaasgroepBadRequest ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepBadRequest>();
			_ = result.As<MaasgroepBadRequest>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepBadRequest>().Message.Should().Be("Declaratie kon niet worden aangemaakt");
		}

		[Fact]
		public void ReceiptAddPhoto_LoggedIn_PermissionCorrect_PhotoCreate_ReturnsCreatedResult()
		{
			SetAuthentication_PermissionCorrect("admin");

			ReceiptPhotoData addPhotoData = new()
			{
				Base64Image = "doet er niet toe",
				FileExtension = "kevin",
				FileName = "hoi!",
				ReceiptId = 1
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};


			ReceiptPhoto photoToReturn = new()
			{
				FileExtension = "test",
				Id = 15,
				ReceiptId = 1
			};



			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(getByIdReceipt);
			receiptPhotoRepository.Setup(p => p.Create(It.IsAny<ReceiptPhotoData>(), It.IsAny<long>())).Returns(photoToReturn);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptAddPhoto(0, addPhotoData);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<CreatedResult>();
			_ = result.As<CreatedResult>().Location.Should().NotBeNull();
			_ = result.As<CreatedResult>().Location.Should().Be($"/api/v1/ReceiptPhotos/{photoToReturn.Id}");
			_ = result.As<CreatedResult>().Value.Should().NotBeNull();
			_ = result.As<CreatedResult>().Value.Should().BeOfType<ReceiptPhoto>();
			_ = result.As<CreatedResult>().Value.As<ReceiptPhoto>().Id.Should().Be(photoToReturn.Id);
		}

		#endregion

		#region ReceiptGetPhotos

		[Fact]
		public void ReceiptGetPhotos_NotLoggedIn_NullData_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptGetPhotos(0, 0, 0, false);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptGetPhotos_NotLoggedIn_ReceiptExists_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			Receipt receiptById = new()
			{
				Id = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receiptById);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptGetPhotos(2, 2, 2, false);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptGetPhotos_WrongPermission_ReceiptExists_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_PermissionWrong();

			Receipt receiptById = new()
			{
				Id = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receiptById);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptGetPhotos(2, 2, 2, false);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptGetPhotos_ValidPermission_ReceiptExists_ReturnsOkObjectResult()
		{
			SetAuthentication_PermissionCorrect("admin");

			Receipt receiptById = new()
			{
				Id = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), false)).Returns(receiptById);
			receiptPhotoRepository.Setup(p => p.ListByReceipt(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(new List<ReceiptPhotoModel>() { new ReceiptPhotoModel() { Id = -2 } });

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptGetPhotos(2, 2, 2, false);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().NotBeNull();
			_ = result.As<OkObjectResult>().Value.As<List<ReceiptPhotoModel>>().Should().NotBeNull();
			_ = result.As<OkObjectResult>().Value.As<List<ReceiptPhotoModel>>().Count.Should().Be(1);
		}

		#endregion

		#region ReceiptGetApprovals
		[Fact]
		public void ReceiptGetApprovals_NotLoggedIn_ReturnsOkResult()
		{
			SetAuthentication_Unauthenticated();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptGetApprovals(3, 3, 3);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().NotBeNull();
		}
		#endregion

		#region ReceiptApprove

		[Fact]
		public void ReceiptApprove_NotLoggedIn_NullData_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			ReceiptApprovalData dataApproved = new()
			{
				Approved = true,
				Note = "Champs 4 life",
				ReceiptId =1
			};

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptApprove(1, dataApproved);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptApprove_NotLoggedIn_ReceiptExists_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			ReceiptApprovalData dataApproved = new()
			{
				Approved = true,
				Note = "Champs 4 life",
				ReceiptId = 1
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), true)).Returns(getByIdReceipt);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptApprove(1, dataApproved);
			}
			catch (MaasgroepNotFound ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}

		[Fact]
		public void ReceiptApprove_LoggedIn_ReceiptExists_CreateNull_ThrowsMaasgroepBadRequestException()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			ReceiptApprovalData dataApproved = new()
			{
				Approved = true,
				Note = "Champs 4 life",
				ReceiptId = 1
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};


			ReceiptApproval returnAproval = null;

			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(getByIdReceipt);
			receiptApprovalRepository.Setup(a => a.Create(dataApproved, It.IsAny<long>())).Returns(returnAproval);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptApprove(1, dataApproved);
			}
			catch (MaasgroepBadRequest ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepBadRequest>();
			_ = result.As<MaasgroepBadRequest>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepBadRequest>().Message.Should().Be($"Declaratie kon niet worden aangemaakt");
		}

		[Fact]
		public void ReceiptApprove_LoggedIn_ReceiptExists_CreateOk_ReturnsOkResult()
		{
			SetAuthentication_PermissionCorrect("receipt.approve");

			ReceiptApprovalData dataApproved = new()
			{
				Approved = true,
				Note = "Champs 4 life",
				ReceiptId = 1
			};

			Receipt getByIdReceipt = new()
			{
				Note = "test",
				Amount = 1,
				CostCentreId = 1
			};


			ReceiptApproval returnAproval = new()
			{
				Id = 89
			};

			ReceiptApprovalModel okModel = new()
			{
				Id = 9123
			};


			receiptRepository.Setup(r => r.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(getByIdReceipt);
			receiptApprovalRepository.Setup(a => a.Create(dataApproved, It.IsAny<long>())).Returns(returnAproval);
			receiptApprovalRepository.Setup(a => a.GetModel(returnAproval)).Returns(okModel);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.ReceiptApprove(1, dataApproved);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().NotBeNull();
			_ = result.As<OkObjectResult>().Value.Should().BeOfType<ReceiptApprovalModel>();
			_ = result.As<OkObjectResult>().Value.As<ReceiptApprovalModel>().Id.Should().Be(9123);
		}

		#endregion

		#region GetPayableReceipts

		[Fact]
		public void GetPayableReceipts_NotLoggedIn_ThrowsMaasgroepUnauthorizedException()
		{
			SetAuthentication_Unauthenticated();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.GetPayableReceipts(1, 1, false);
			}
			catch (MaasgroepUnauthorized ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepUnauthorized>();
			_ = result.As<MaasgroepUnauthorized>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepUnauthorized>().Message.Should().Be($"Je bent niet ingelogd");
		}

		[Fact]
		public void GetPayableReceipts_InvalidPermissions_ThrowsMaasgroepForbiddenException()
		{
			SetAuthentication_PermissionWrong();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.GetPayableReceipts(1, 1, false);
			}
			catch (MaasgroepForbidden ex)
			{
				result = ex;
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<MaasgroepForbidden>();
			_ = result.As<MaasgroepForbidden>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepForbidden>().Message.Should().Be($"Je hebt geen toegang tot dit onderdeel");
		}

		[Fact]
		public void GetPayableReceipts_ValidPermissions_ReturnsOkResult()
		{
			SetAuthentication_PermissionCorrect("receipt.pay");

			var okResult = new List<ReceiptModel>()
			{ 
				new ReceiptModel()
				{
					Id = 95889234
				}
			};

			receiptRepository.Setup(r => r.ListPayable(It.IsAny<int>(), It.IsAny<int>(), false)).Returns(okResult);

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.GetPayableReceipts(1, 1, false);
			}
			catch (Exception ex)
			{
				result = ex;
			}

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().NotBeNull();
			_ = result.As<OkObjectResult>().Value.Should().BeOfType<List<ReceiptModel>>();
			_ = result.As<OkObjectResult>().Value.As<List<ReceiptModel>>().Count.Should().Be(1);
			_ = result.As<OkObjectResult>().Value.As<List<ReceiptModel>>().ElementAt(0).Should().BeOfType<ReceiptModel>();
			_ = result.As<OkObjectResult>().Value.As<List<ReceiptModel>>().ElementAt(0).As<ReceiptModel>().Id.Should().Be(95889234);
		}

		#endregion


		private void SetAuthentication_Unauthenticated()
		{
			MemberModel fakeMaasgroepMember = null;
			maasgroepAuthenticationService.Setup(m => m.GetCurrentMember(null)).Returns(fakeMaasgroepMember);
		}

		private void SetAuthentication_PermissionWrong()
		{
			var fakeMaasgroepMember = new MemberModel()
			{
				Id = 1,
				Email = "mockEmail@mockMember.com",
				Name = "mockMember",
				Permissions = new List<string>() { "wrong.permission" }
			};

			maasgroepAuthenticationService.Setup(m => m.GetCurrentMember(null)).Returns(fakeMaasgroepMember);
		}

		private void SetAuthentication_PermissionCorrect(string permission) 
		{
			var fakeMaasgroepMember = new MemberModel()
			{
				Id = 1,
				Email = "mockEmail@mockMember.com",
				Name = "mockMember",
				Permissions = new List<string>() { permission }
			};

			maasgroepAuthenticationService.Setup(m => m.GetCurrentMember(null)).Returns(fakeMaasgroepMember);
		}


	}
}