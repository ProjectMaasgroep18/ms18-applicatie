using FluentAssertions;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;
using Moq;
using ms18_applicatie.Controllers.Api;
using ms18_applicatie.Services;

namespace ms18_applicatie_test
{
	public class CostCentreControllerTest
	{
		#region CostCentreGet

		public void Access_CostCentreGet_Without_Authentication_Result_Forbidden()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGet();

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		public void Access_CostCentreGet_With_Authentication_Result_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentres(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<CostCentreModel>());

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGet();

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkResult>();
		}

		#endregion

		#region CostCentreGetById
		[Fact]
		public void Access_CostCentreGetById_Without_Authentication_Result_Forbidden()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGetById(It.IsAny<int>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_CostCentreGetById_With_Authentication_ResultNull_Returns_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns<CostCentreModel>(null);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGetById(It.IsAny<int>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
		}

		[Fact]
		public void Access_CostCentreGetById_With_Authentication_ResultFound_Returns_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(new Mock<CostCentreModel>().Object);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGetById(It.IsAny<int>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
		}
		#endregion

		#region CostCentreCreate

		[Fact]
		public void Access_CostCentreCreate_Without_Authentication_Result_Forbidden()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreCreate(new Mock<CostCentreModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_CostCentreCreate_With_Authentication_InvalidModelState_Result_BadRequest()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);
			sut.ModelState.AddModelError("blaat", "kapot!");

			var result = sut.CostCentreCreate(new Mock<CostCentreModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<BadRequestObjectResult>();
		}

		[Fact]
		public void Access_CostCentreCreate_With_Authentication_Valid_Model_Result_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.Add(new Mock<CostCentreModelCreateDb>().Object)).Returns(It.IsAny<long>());

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);
			sut.ModelState.AddModelError("blaat", "kapot!");

			var result = sut.CostCentreCreate(new Mock<CostCentreModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
		}

		#endregion

		#region CostCentreUpdate

		[Fact]
		public void Access_CostCentreUpdate_Without_Authentication_Result_Forbidden()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreUpdate(It.IsAny<long>(), new Mock<CostCentreModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_CostCentreUpdate_With_Authentication_InvalidModelState_Result_BadRequest()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);
			sut.ModelState.AddModelError("blaat", "kapot!");

			var result = sut.CostCentreUpdate(It.IsAny<long>(), new Mock<CostCentreModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<BadRequestObjectResult>();
		}

		[Fact]
		public void Access_CostCentreUpdate_With_Authentication_CentreNotFound_Result_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(new Mock<CostCentreModel>().Object);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreUpdate(It.IsAny<long>(), new Mock<CostCentreModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
		}

		[Fact]
		public void Access_CostCentreUpdate_With_Authentication_CentreFoundButExists_Result_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);

			var costCentreEqual = new Mock<CostCentreModel>().Object;

			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(costCentreEqual);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreUpdate(It.IsAny<long>(), costCentreEqual);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().Be(new
			{
				status = 200,
				message = "Er zijn geen wijzigingen aan de kostenpost"
			});

			var q = "hoi";
		}

		#endregion

		#region CostCentreDelete
		#endregion

		#region CostCentreGetReceipts
		#endregion
	}
}
