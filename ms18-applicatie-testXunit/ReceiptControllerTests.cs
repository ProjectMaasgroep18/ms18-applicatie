using Maasgroep.SharedKernel.Interfaces.Receipts;
using Moq;
using ms18_applicatie.Services;
using ms18_applicatie.Controllers.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Maasgroep.SharedKernel.ViewModels.Admin;

namespace ms18_applicatie_test
{
	public class ReceiptControllerTests
	{

		#region ReceiptGet
		[Fact]
		public void Access_ReceiptGet_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGet();

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ReceiptGet_With_Authentication_With_Receipts_Null_Gives_NotFoundObject()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipts(It.IsAny<int>(), It.IsAny<int>())).Returns<IEnumerable<ReceiptModel>>(null);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGet();

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Receipt not found"
				}
				);
		}

		[Fact]
		public void Access_ReceiptGet_With_Authentication_With_Receipts_Zero_Gives_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipts(It.IsAny<int>(), It.IsAny<int>())).Returns(new Mock<List<ReceiptModel>>().Object);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGet(); //Count van de lijst is null, maar komt nu nog door.

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		}

		[Fact]
		public void Access_ReceiptGet_With_Authentication_With_Receipts_OneOrMore_Gives_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipts(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<ReceiptModel>() { new Mock<ReceiptModel>().Object, new Mock<ReceiptModel>().Object });

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGet(); //Count van de lijst is null, maar komt nu nog door.

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		}

		#endregion

		#region ReceiptGetById
		[Fact]
		public void Access_ReceiptGetById_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetById(It.IsAny<int>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ReceiptGetById_With_Authentication_With_Receipt_Null_Gives_NotFoundObject()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<int>())).Returns<ReceiptModel>(null);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetById(It.IsAny<int>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Receipt not found"
				}
				);
		}

		[Fact]
		public void Access_ReceiptGetById_With_Authentication_With_Receipt_Found_Gives_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns(new ReceiptModel());

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetById(It.IsAny<int>()); //Count van de lijst is null, maar komt nu nog door.

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
		}
		#endregion

		#region ReceiptCreate

		[Fact]
		public void Access_ReceiptCreate_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptCreate(new Mock<ReceiptModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ReceiptCreate_With_Authentication_Invalid_Model_Is_BadRequest()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);


			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);
			sut.ModelState.AddModelError("blaat", "kapot!");

			var result = sut.ReceiptCreate(new Mock<ReceiptModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<BadRequestObjectResult>();
			_ = result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 400,
					message = "Invalid request body"
				}
				);
		}

		[Fact]
		public void Access_ReceiptCreate_With_Authentication_Valid_Model_Add_Returns_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = memberService.Setup(p => p.GetMember(It.IsAny<long>())).Returns(new Mock<MemberModel>().Object);
			_ = receiptRepository.Setup(p => p.Add(new Mock<ReceiptModelCreateDb>().Object)).Returns(-1L);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptCreate(new Mock<ReceiptModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		}
		#endregion

		#region ReceiptUpdate
		[Fact]
		public void Access_ReceiptUpdate_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptUpdate(new Mock<ReceiptModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ReceiptUpdate_With_Authentication_Invalid_Model_Is_BadRequest()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);
			sut.ModelState.AddModelError("blaat", "kapot!");

			var result = sut.ReceiptUpdate(new Mock<ReceiptModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<BadRequestObjectResult>();
			_ = result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 400,
					message = "Invalid request body"
				}
				);
		}

		[Fact]
		public void Access_ReceiptUpdate_With_Authentication_Valid_Model_Add_Returns_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = memberService.Setup(p => p.GetMember(It.IsAny<long>())).Returns(new Mock<MemberModel>().Object);
			_ = receiptRepository.Setup(p => p.Modify(new Mock<ReceiptModelUpdateDb>().Object)).Returns(false);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptUpdate(new Mock<ReceiptModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		}

		#endregion

		#region ReceiptDelete

		[Fact]
		public void Access_ReceiptDelete_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGet();

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ReceiptDelete_With_Authentication_Receipt_NotFound_Returns_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<int>())).Returns<ReceiptModel>(null);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Receipt not found"
				}
				);
		}

		[Fact]
		public void Access_ReceiptDelete_With_Authentication_Delete_Attempt_False_Returns_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(r => r.Delete(It.IsAny<ReceiptModelDeleteDb>())).Returns(false);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NoContentResult>();
			_ = result.As<NoContentResult>().StatusCode.Should().Be(204);
		}

		[Fact]
		public void Access_ReceiptDelete_With_Authentication_Delete_Attempt_Exception_Returns_Conflict()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(r => r.Delete(It.IsAny<ReceiptModelDeleteDb>())).Throws(new Exception());

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ConflictObjectResult>();
			_ = result.As<ConflictObjectResult>().StatusCode.Should().Be(409);
			_ = result.As<ConflictObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 409,
					message = "Declaratie kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
				}
				);
		}

		[Fact]
		public void Access_ReceiptDelete_With_Authentication_Delete_Attempt_Successful_Return_NoContent()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(r => r.Delete(It.IsAny<ReceiptModelDeleteDb>())).Returns(true);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NoContentResult>();
		}

		#endregion

		#region ReceiptAddPhoto

		[Fact]
		public void Access_ReceiptAddPhoto_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGet();

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ReceiptAddPhoto_With_Authentication_Invalid_Model_Is_BadRequest()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);
			sut.ModelState.AddModelError("blaat", "kapot!");

			var result = sut.ReceiptAddPhoto(It.IsAny<long>(), new Mock<PhotoModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<BadRequestObjectResult>();
			_ = result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 400,
					message = "Invalid request body"
				}
				);
		}

		[Fact]
		public void Access_ReceiptAddPhoto_With_Authentication_GetReceipt_Null_Is_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetReceipt(It.IsAny<long>())).Returns<ReceiptModel>(null);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptAddPhoto(It.IsAny<long>(), new Mock<PhotoModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Receipt not found"
				});
		}

		[Fact]
		public void Access_ReceiptAddPhoto_With_Authentication_GetReceipt_Found_Is_CreatedResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			var photoExpected = It.IsAny<PhotoModel>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(p => p.Add(new Mock<PhotoModelCreateDb>().Object)).Returns(It.IsAny<long>());
			_ = receiptRepository.Setup(p => p.GetPhoto(It.IsAny<long>())).Returns(photoExpected);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptAddPhoto(It.IsAny<long>(), new Mock<PhotoModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<CreatedResult>();
			_ = result.As<CreatedResult>().StatusCode.Should().Be(201);
			_ = result.As<CreatedResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 201,
					message = "Photo created",
					photo = photoExpected
				}
				);
		}

		#endregion

		#region ReceiptGetPhotos

		[Fact]
		public void Access_ReceiptGetPhotos_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetPhotos(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ReceiptGetPhotos_With_Authentication_Receipt_NotFound_Returns_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns((ReceiptModel)null);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetPhotos(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Receipt not found"
				}
				);

		}

		[Fact]
		public void Access_ReceiptGetPhotos_With_Authentication_Receipt_Found_Returns_EmptyPhotos_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(r => r.GetPhotosByReceipt(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns<IEnumerable<PhotoModel>>(null);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetPhotos(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		}

		[Fact]
		public void Access_ReceiptGetPhotos_With_Authentication_Receipt_Found_Returns_0Photos_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(r => r.GetPhotosByReceipt(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<PhotoModel>());

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetPhotos(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		}

		[Fact]
		public void Access_ReceiptGetPhotos_With_Authentication_Receipt_Found_Returns_1orMorePhotos_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(r => r.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(r => r.GetPhotosByReceipt(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<PhotoModel>() { new Mock<PhotoModel>().Object });

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGetPhotos(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		}

		#endregion

		#region ApproveReceipt

		[Fact]
		public void Access_ApproveReceipt_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ApproveReceipt(It.IsAny<long>(), new Mock<ReceiptApprovalModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_ApproveReceipt_With_Authentication_And_Invalid_Model_Is_BadRequest()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);
			sut.ModelState.AddModelError("blaat", "kapot!");

			var result = sut.ApproveReceipt(It.IsAny<long>(), new Mock<ReceiptApprovalModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<BadRequestObjectResult>();
			_ = result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 400,
					message = "Invalid request body"
				}
				);

		}

		[Fact]
		public void Access_ApproveReceipt_With_Authentication_Receipt_Not_Found_Results_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetReceipt(It.IsAny<long>())).Returns<ReceiptModel>(null);

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ApproveReceipt(It.IsAny<long>(), new Mock<ReceiptApprovalModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().StatusCode.Should().Be(404);
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Receipt not found"
				}
				);
		}

		[Fact]
		public void Access_ApproveReceipt_With_Authentication_Receipt_Found_Results_Created()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetReceipt(It.IsAny<long>())).Returns(new Mock<ReceiptModel>().Object);
			_ = receiptRepository.Setup(p => p.AddApproval(new Mock<ReceiptApprovalModelCreateDb>().Object));

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ApproveReceipt(It.IsAny<long>(), new Mock<ReceiptApprovalModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<CreatedResult>();
			_ = result.As<CreatedResult>().StatusCode.Should().Be(201);
		}
		#endregion

	}
}