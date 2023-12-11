using Microsoft.AspNetCore.Mvc;
using Maasgroep.SharedKernel.Interfaces.Orders;
using Maasgroep.SharedKernel.ViewModels.Orders;
using ms18_applicatie.Services;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
	private readonly IOrderRepository _orderRepository;
	private readonly IMemberService _memberService;
	public StockController(IOrderRepository orderRepository, IMemberService memberService)
	{
		_orderRepository = orderRepository;
		_memberService = memberService;
	}

	[HttpGet("{productId}")]
    [ActionName("stockGet")]
    public IActionResult StockGet(long productId)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		var productje = _orderRepository.GetProduct(productId);

        //if (dbRecProduct == null)
        //    return NotFound(new
        //    {
        //        status = 404,
        //        message = "Product niet gevonden"
        //    });

        var stockje = _orderRepository.GetStock(productId);
		var result = new StockModel()
		{
			Product = productje,
			Quantity = stockje.Quantity
		};

		if (stockje == null)
        {
            try
            {
                CreateStockIfNotExists(productId);
            }
            catch (Exception) 
            {
				return BadRequest(new
				{
					status = 422,
					message = "Kon voorraad niet aanmaken"
				});
			}
        }

        return Ok(result);
    }

    [HttpPut("{productId}/Increase")]
    [ActionName("stockIncrease")]
    public IActionResult StockIncrease(long productId, [FromBody] StockModel quantityModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Validate the request body
		if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body"
            });
        }

        var dbRecProduct = _orderRepository.GetProduct(productId);

        if (dbRecProduct == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        var dbRec = _orderRepository.GetStock(productId);

        if (dbRec == null)
        {
			try
			{
				CreateStockIfNotExists(productId);
			}
			catch (Exception)
			{
				return BadRequest(new
				{
					status = 422,
					message = "Kon voorraad niet aanmaken"
				});
			}
		}
        else
        {
            var nieuweStock = new StockModelUpdateDb()
            {
                Stock = quantityModel,
				Member = _memberService.GetMember(1)
            };

            // Try and add the stock to the database and return it
            try
            {
                _orderRepository.Modify(nieuweStock);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(new
                {
                    status = 422,
                    message = "Kon voorraad niet wijzigen"
                });
            }
        }

        return NoContent();
    }

    [HttpPut("{productId}/Decrease")]
    [ActionName("stockDecrease")] // KH: Volgens mij is het niet nodig voor quantiy aanpassen 2 operites?
    public IActionResult StockDecrease(long productId, [FromBody] StockModel quantityModel)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Validate the request body
		if (!ModelState.IsValid)
		{
			return BadRequest(new
			{
				status = 400,
				message = "Invalid request body"
			});
		}

		var dbRecProduct = _orderRepository.GetProduct(productId);

		if (dbRecProduct == null)
			return NotFound(new
			{
				status = 404,
				message = "Product niet gevonden"
			});

		var dbRec = _orderRepository.GetStock(productId);

		if (dbRec == null)
		{
			try
			{
				CreateStockIfNotExists(productId);
			}
			catch (Exception)
			{
				return BadRequest(new
				{
					status = 422,
					message = "Kon voorraad niet aanmaken"
				});
			}
		}
		else
		{
			var nieuweStock = new StockModelUpdateDb()
			{
				Stock = quantityModel,
				Member = _memberService.GetMember(1)
			};

			// Try and add the stock to the database and return it
			try
			{
				_orderRepository.Modify(nieuweStock);
			}
			catch (Exception e)
			{
				return UnprocessableEntity(new
				{
					status = 422,
					message = "Kon voorraad niet wijzigen"
				});
			}
		}

		return NoContent();
	}

	private bool MemberExists(long id) => _memberService.MemberExists(id);
	private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");

	// KH; ik zou niet stock aanmaken als het niet bestaat maar fout teruggeven en het aan een anedr laten.
    private long CreateStockIfNotExists(long id)
    {
        long result = 0;
		// If there is no stock for this product, create empty stock
		var nieuweStock = new StockModelCreateDb()
		{
			Stock = new StockModelCreate() { Quantity = 0, ProductId = id },
			Member = _memberService.GetMember(1)
		};


		// Try and add the stock to the database and return it
		result = _orderRepository.Add(nieuweStock);

        return result;
	}

}