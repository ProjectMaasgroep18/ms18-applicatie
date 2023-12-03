using Microsoft.AspNetCore.Mvc;
using Maasgroep.SharedKernel.Interfaces.Orders;
using Maasgroep.SharedKernel.ViewModels.Order;
using ms18_applicatie.Services;

namespace ms18_applicatie.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
		private readonly IOrderRepository _orderRepository;
		private readonly IMemberService _memberService;
		public OrderController(IOrderRepository orderRepository, IMemberService memberService)
		{
			_orderRepository = orderRepository;
			_memberService = memberService;
		}


		[HttpGet]
        [ActionName("stockGet")]
        public IActionResult StockGet()
        {
			if (!MemberExists(1)) // Toegangscontrole
				return Forbidden();

            var allStock = _orderRepository.GetStock(0, int.MaxValue);

            return Ok(allStock);
        }

        [HttpGet]
        [ActionName("stockGetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult StockGetById(long id)
        {
			if (!MemberExists(1)) // Toegangscontrole
				return Forbidden();

            var stockCurrent = _orderRepository.GetStock(id);
            //TODO: lijst opvragen, groter dan 1, etc
            return Ok(stockCurrent);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ModifyStock(StockModel stock)
        {
			if (!MemberExists(1)) // Toegangscontrole
				return Forbidden();

			if (stock.Quantity < 0)
            {
                return BadRequest("Quantity must be equal or greater than 0.");
            }

            //KH: stock heeft foreign key naar product.
            //    dus als product bestaat, bestaat stock.
            var product = _orderRepository.GetProduct(stock.Product.Id);

            if (product == null)
            {
                return BadRequest("Dit moet nog gedaan worden, toevoegen");
            }
            else
            {
                var updatedStock = new StockModelUpdateDb()
                {
                    Stock = stock,
                    Member = _memberService.GetMember(1)
                };

                _orderRepository.Modify(updatedStock);

                return Created("", _orderRepository);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteStock(long id)
        {
			if (!MemberExists(1)) // Toegangscontrole
				return Forbidden();

            var stock = _orderRepository.GetStock(id);

            // if null

            var result = _orderRepository.Delete(stock);

			return Ok(result);
        }

		private bool MemberExists(long id) => _memberService.MemberExists(id);
		private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
	}


}
