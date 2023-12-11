using Microsoft.AspNetCore.Mvc;
using Maasgroep.SharedKernel.Interfaces.Orders;
using Maasgroep.SharedKernel.ViewModels.Orders;
using ms18_applicatie.Services;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMemberService _memberService;
    public ProductController(IOrderRepository orderRepository, IMemberService memberService)
    {
        _orderRepository = orderRepository;
		_memberService = memberService;
    }

    [HttpGet]
    [ActionName("productGet")]
    public IActionResult ProductGet()
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        var products = _orderRepository.GetProducts(0, int.MaxValue);

        return Ok(products);
    }

    [HttpGet("{id}")]
    [ActionName("productGetById")]
    public IActionResult ProductGetById(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        var product = _orderRepository.GetProduct(id);

        if (product == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        return Ok(product);
    }
    
    [HttpPost]
    [ActionName("productCreate")]
    public IActionResult ProductCreate([FromBody] ProductModelCreate product)
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

        var createProduct = new ProductModelCreateDb()
        {
            Product = product,
            Member = _memberService.GetMember(1)
        };

        long productId = -1;
        try
        {
            productId = _orderRepository.Add(createProduct);
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                status = 422,
                message = "Kon product niet aanmaken",
            });
        }

        return Ok(new
        {
            status = 200,
            message = "Product aangemaakt",
            product = productId
		});
    }
    
    [HttpPut("{id}")]
    [ActionName("productUpdate")]
    public IActionResult ProductUpdate(long id, [FromBody] ProductModel product)
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
        
        var existingProduct = _orderRepository.GetProduct(id);

        if (existingProduct == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });
        }

        if (existingProduct.Equals(product))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "Product niet gewijzigd"
            });
        }

        var updatedProduct = new ProductModelUpdateDb()
        {
            Product = product,
            Member = _memberService.GetMember(1)
        };

        _orderRepository.Modify(updatedProduct);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ActionName("productDelete")]
    public IActionResult ProductDelete(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

        var existingProduct = _orderRepository.GetProduct(id);

        if (existingProduct == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        var productToDelete = new ProductModelDeleteDb()
        {
            Product = existingProduct,
            Member = _memberService.GetMember(1)
        };

        try
        {
            _orderRepository.Delete(productToDelete);
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                status = 409,
                message = "Kon product niet verwijderen",
            });
        }

        return NoContent();
    }

    //TODO: Deze functies staan in elke controller, hangt samen met check op user.
	private bool MemberExists(long id) => _memberService.MemberExists(id);
	private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
}