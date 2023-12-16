using FluentAssertions;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Maasgroep.SharedKernel.ViewModels.Receipts;
using Microsoft.AspNetCore.Mvc;
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
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Kostenpost niet gevonden"
				}
				);
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
			_ = result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 400,
					message = "Invalid request body"
				});
		}

		[Fact]
		public void Access_CostCentreCreate_With_Authentication_Valid_Model_Result_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.Add(It.IsAny<CostCentreModelCreateDb>())).Returns(123L);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreCreate(new Mock<CostCentreModelCreate>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<CreatedResult>();
			_ = result.As<CreatedResult>().Value.Should().BeEquivalentTo(new
			{
				status = 201,
				message = "Kostenpost aangemaakt",
				costCentre = 123L,
			});

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
			_ = result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 400,
					message = "Invalid request body"
				}
			);
		}

		[Fact]
		public void Access_CostCentreUpdate_With_Authentication_CentreNotFound_Result_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns<CostCentreModel>(null);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreUpdate(It.IsAny<long>(), new Mock<CostCentreModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Kostenpost niet gevonden"
				}
			);
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
			_ = result.As<OkObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 200,
					message = "Er zijn geen wijzigingen aan de kostenpost"
				}
			);
		}

		[Fact]
		public void Access_CostCentreUpdate_With_Authentication_CentreFoundNotEqual_Result_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);

			var centreErin = new Mock<CostCentreModel>().Object;
			centreErin.Name = "pietje";
			
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(centreErin);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreUpdate(It.IsAny<long>(), new Mock<CostCentreModel>().Object);

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().Be(false);
		}

		#endregion

		#region CostCentreDelete

		[Fact]
		public void Access_CostCentreDelete_Without_Authentication_Result_Forbidden()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_CostCentreDelete_With_Authentication_CostCentreNotFound_Result_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns<CostCentreModel>(null);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Kostenpost niet gevonden"
				}
			);
		}

		[Fact]
		public void Access_CostCentreDelete_With_Authentication_CostCentreFound_DeleteFailed_Result_Conflict()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(new Mock<CostCentreModel>().Object);
			_ = receiptRepository.Setup(p => p.Delete(It.IsAny<CostCentreModelDeleteDb>())).Throws(new Exception());

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ConflictObjectResult>();
			_ = result.As<ConflictObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 409,
					message = "Kostenpost kon niet worden verwijderd" // TODO Check which dependency is causing the conflict
				}
			);
		}

		[Fact]
		public void Access_CostCentreDelete_With_Authentication_CostCentreFound_NoConflict_Result_NoContent()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(new Mock<CostCentreModel>().Object);
			_ = receiptRepository.Setup(p => p.Delete(new Mock<CostCentreModelDeleteDb>().Object)).Returns(true);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreDelete(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NoContentResult>();
		}

		#endregion

		#region CostCentreGetReceipts

		[Fact]
		public void Access_CostCentreGetReceipts_Without_Authentication_Result_Forbidden()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGetReceipts(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<ForbidResult>();
			_ = result.As<ForbidResult>().AuthenticationSchemes.Should().BeEquivalentTo("Je hebt geen toegang tot deze functie");
		}

		[Fact]
		public void Access_CostCentreGetReceipts_With_Authentication_CostCentreNotFound_Result_NotFound()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns<CostCentreModel>(null);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGetReceipts(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<NotFoundObjectResult>();
			_ = result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(
				new
				{
					status = 404,
					message = "Kostenpost niet gevonden"
				}
			);
		}

		[Fact]
		public void Access_CostCentreGetReceipts_With_Authentication_CostCentreFound_ReceiptsNot_Result_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(new Mock<CostCentreModel>().Object);
			_ = receiptRepository.Setup(p => p.GetReceiptsByCostCentre(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns<IEnumerable<ReceiptModel>>(null);

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGetReceipts(It.IsAny<long>());
			
			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().BeNull();//TODO dit is dus echt niet goedd.
		}

		[Fact]
		public void Access_CostCentreGetReceipts_With_Authentication_CostCentreFound_ReceiptsOk_Result_Ok()
		{
			var receiptRepository = new Mock<IReceiptRepository>();
			var memberService = new Mock<IMemberService>();
			_ = memberService.Setup(p => p.MemberExists(It.IsAny<long>())).Returns(true);
			_ = receiptRepository.Setup(p => p.GetCostCentre(It.IsAny<long>())).Returns(new Mock<CostCentreModel>().Object);
			_ = receiptRepository.Setup(p => p.GetReceiptsByCostCentre(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<ReceiptModel>() { new Mock<ReceiptModel>().Object });

			var sut = new CostCentreController(receiptRepository.Object, memberService.Object);

			var result = sut.CostCentreGetReceipts(It.IsAny<long>());

			_ = result.Should().NotBeNull();
			_ = result.Should().BeOfType<OkObjectResult>();
			_ = result.As<OkObjectResult>().Value.Should().NotBeNull();
		}

		#endregion
	}
}
