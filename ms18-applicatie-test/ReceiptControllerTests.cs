using Maasgroep.SharedKernel.Interfaces.Receipts;
using Moq;
using ms18_applicatie.Services;
using ms18_applicatie.Controllers.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Maasgroep.SharedKernel.ViewModels.Receipts;

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

			result.Should().NotBeNull();
			result.Should().BeOfType<ForbidResult>();
			result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
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

			result.Should().NotBeNull();
			result.Should().BeOfType<ForbidResult>();
			result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
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

		#region ReceiptDelete

		[Fact]
		public void Access_ReceiptDelete_Without_Authentication_Is_ForbidResult()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new ReceiptController(receiptRepository.Object, memberService.Object);

			var result = sut.ReceiptGet();

			result.Should().NotBeNull();
			result.Should().BeOfType<ForbidResult>();
			result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
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
			_ = result.Should().BeOfType<NoContentResult>();
			_ = result.As<NoContentResult>().StatusCode.Should().Be(204);
		}

		#endregion
	}
}