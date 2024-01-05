using FluentAssertions;
using Maasgroep.SharedKernel.Interfaces;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Maasgroep.Controllers.Api;

namespace ms18_applicatie_test
{
	public class ReceiptPhotoControllerTests
	{

		//#region PhotoGet

		//[Fact]
		//public void Access_PhotoGet_Without_Authentication_Is_ForbidResult()
		//{
		//	var receiptRepository = new Mock<IReceiptRepository>();
		//	var memberService = new Mock<IMemberService>();

		//	var sut = new ReceiptPhotoController(receiptRepository.Object, memberService.Object);

		//	var result = sut.PhotoGet(It.IsAny<long>());

		//	_ = result.Should().NotBeNull();
		//	_ = result.Should().BeOfType<ForbidResult>();
		//	_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		//}

		//[Fact]
		//public void Access_PhotoGet_With_Authentication_PhotoNull_Result_NotFound()
		//{
		//	var receiptRepository = new Mock<IReceiptRepository>();
		//	var memberService = new Mock<IMemberService>();
		//	_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
		//	_ = receiptRepository.Setup(r => r.GetPhoto(It.IsAny<long>())).Returns<PhotoModel>(null);

		//	var sut = new ReceiptPhotoController(receiptRepository.Object, memberService.Object);

		//	var result = sut.PhotoGet(It.IsAny<long>());

		//	_ = result.Should().NotBeNull();
		//	_ = result.Should().BeOfType<NotFoundObjectResult>();
		//	_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
		//		new
		//		{
		//			status = 404,
		//			message = "Photo not found"
		//		});
		//}

		//[Fact]
		//public void Access_PhotoGet_With_Authentication_PhotoExist_Result_Ok()
		//{
		//	var receiptRepository = new Mock<IReceiptRepository>();
		//	var memberService = new Mock<IMemberService>();
		//	_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
		//	_ = receiptRepository.Setup(r => r.GetPhoto(It.IsAny<long>())).Returns(new Mock<PhotoModel>().Object);

		//	var sut = new ReceiptPhotoController(receiptRepository.Object, memberService.Object);

		//	var result = sut.PhotoGet(It.IsAny<long>());

		//	_ = result.Should().NotBeNull();
		//	_ = result.Should().BeOfType<OkObjectResult>();
		//	_ = result.As<OkObjectResult>().StatusCode.Should().Be(200);
		//}

		//#endregion

		//#region PhotoDelete

		//[Fact]
		//public void Access_PhotoDelete_Without_Authentication_Is_ForbidResult()
		//{
		//	var receiptRepository = new Mock<IReceiptRepository>();
		//	var memberService = new Mock<IMemberService>();

		//	var sut = new ReceiptPhotoController(receiptRepository.Object, memberService.Object);

		//	var result = sut.PhotoDelete(It.IsAny<long>());

		//	_ = result.Should().NotBeNull();
		//	_ = result.Should().BeOfType<ForbidResult>();
		//	_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		//}

		//[Fact]
		//public void Access_PhotoDelete_With_Authentication_PhotoNull_Result_NotFound()
		//{
		//	var receiptRepository = new Mock<IReceiptRepository>();
		//	var memberService = new Mock<IMemberService>();
		//	_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
		//	_ = receiptRepository.Setup(r => r.GetPhoto(It.IsAny<long>())).Returns<PhotoModel>(null);

		//	var sut = new ReceiptPhotoController(receiptRepository.Object, memberService.Object);

		//	var result = sut.PhotoDelete(It.IsAny<long>());

		//	_ = result.Should().NotBeNull();
		//	_ = result.Should().BeOfType<NotFoundObjectResult>();
		//	_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
		//		new
		//		{
		//			status = 404,
		//			message = "Photo not found"
		//		});
		//}

		//[Fact]
		//public void Access_PhotoDelete_With_Authentication_PhotoDeleteException_Result_Conflict()
		//{
		//	var receiptRepository = new Mock<IReceiptRepository>();
		//	var memberService = new Mock<IMemberService>();
		//	_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
		//	_ = receiptRepository.Setup(r => r.GetPhoto(It.IsAny<long>())).Returns(new Mock<PhotoModel>().Object);
		//	_ = receiptRepository.Setup(p => p.Delete(It.IsAny<PhotoModelDeleteDb>())).Throws(new Exception());

		//	var sut = new ReceiptPhotoController(receiptRepository.Object, memberService.Object);

		//	var result = sut.PhotoDelete(It.IsAny<long>());

		//	_ = result.Should().NotBeNull();
		//	_ = result.Should().BeOfType<ConflictObjectResult>();
		//	_ = result.As<ConflictObjectResult>().StatusCode.Should().Be(409);
		//	_ = result.As<ConflictObjectResult>().Value.Should().BeEquivalentTo(
		//		new
		//		{
		//			status = 409,
		//			message = "Foto kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
		//		}
		//		);
		//}

		//[Fact]
		//public void Access_PhotoDelete_With_Authentication_PhotoDeleteOk_Result_NoContent()
		//{
		//	var receiptRepository = new Mock<IReceiptRepository>();
		//	var memberService = new Mock<IMemberService>();
		//	_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
		//	_ = receiptRepository.Setup(r => r.GetPhoto(It.IsAny<long>())).Returns(new Mock<PhotoModel>().Object);
		//	_ = receiptRepository.Setup(p => p.Delete(It.IsAny<PhotoModelDeleteDb>())).Returns(true);

		//	var sut = new ReceiptPhotoController(receiptRepository.Object, memberService.Object);

		//	var result = sut.PhotoDelete(It.IsAny<long>());

		//	_ = result.Should().NotBeNull();
		//	_ = result.Should().BeOfType<NoContentResult>();
		//	_ = result.As<NoContentResult>().StatusCode.Should().Be(204);
		//}

		//#endregion

	}
}
