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
		/*
		Methodes te testen;
		- ReceiptAddPhoto		ReceiptController
		- ReceiptGetPhotos		ReceiptController
		- ReceiptGetApprovals	ReceiptController
		- ReceiptApprove		ReceiptController
		- GetPayableReceipts	ReceiptController
		*/

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
		public void RepositoryDelete_NotLoggedIn_ThrowsMaasgroepNotFoundException()
		{
			SetAuthentication_Unauthenticated();

			var sut = new ReceiptController(receiptRepository.Object, receiptPhotoRepository.Object, receiptApprovalRepository.Object, maasgroepAuthenticationService.Object);

			object result;
			try
			{
				result = sut.RepositoryDelete(0);
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
			_ = result.Should().BeOfType<MaasgroepNotFound>();
			_ = result.As<MaasgroepNotFound>().Message.Should().NotBeNull();
			_ = result.As<MaasgroepNotFound>().Message.Should().Be($"Declaratie niet gevonden");
		}


		#endregion

		#region RepositoryUpdate

		[Fact]
		public void RepositoryUpdate_NotLoggedIn_ThrowsMaasgroepUnauthorizedException()
		{

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