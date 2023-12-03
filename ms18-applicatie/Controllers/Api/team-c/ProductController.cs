using Maasgroep.Database;
using Maasgroep.Database.Repository.ViewModel;
using Maasgroep.Database.Order;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly StockContext _context;
    public ProductController(StockContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ActionName("productGet")]
    public IActionResult ProductGet()
    {
        //if (_currentUser == null) // Toegangscontrole
        //    return Forbidden();

        var all = _context.Product
            .Select(dbRec => new ProductViewModel(dbRec))
            .ToList()
            .Select(x => x)
            .ToList()
            ;

        return Ok(all);
    }

    [HttpGet("{id}")]
    [ActionName("productGetById")]
    public IActionResult ProductGetById(long id)
    {
        //if (_currentUser == null) // Toegangscontrole
        //    return Forbidden();

        var dbRec = _context.Product
            .Where(dbRec => dbRec.Id == id)
            .Select(dbRec => new ProductViewModel(dbRec))
            .FirstOrDefault();

        if (dbRec == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        return Ok();
        //return Ok(AddForeignData(dbRec));
    }
    
    [HttpPost]
    [ActionName("productCreate")]
    public IActionResult ProductCreate([FromBody] ProductViewModel vm)
    {
        
        //if (_currentUser == null) // Toegangscontrole
        //    return Forbidden();

        // Validate the request body
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body"
            });
        }
        
        var createdProduct = new Product
        {
            Name = vm.Name,
            MemberCreatedId = -1
        };

        try
        {
            _context.Product.Add(createdProduct);
            _context.SaveChanges();
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
            product = createdProduct
            //product = AddForeignData(new ProductViewModel(createdProduct))
        });
    }
    
    [HttpPut("{id}")]
    [ActionName("productUpdate")]
    public IActionResult ProductUpdate(long id, [FromBody] ProductViewModel vm)
    {
        //if (_currentUser == null) // Toegangscontrole
        //    return Forbidden();

        // Validate the request body
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body"
            });
        }
        
        if (id != vm.Id)
            return BadRequest(new
            {
                status = 400,
                message = "Invalid request body, ID in URL does not match ID in request body"
            });

        var existingProduct = _context.Product
            .FirstOrDefault(dbRec => dbRec.Id == id);

        if (existingProduct == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });
        }

        if (ProductsAreEqual(existingProduct, vm))
        {
            // If the data is the same, return a response indicating no update was performed
            return Ok(new {
                status = 200,
                message = "Product niet gewijzigd"
            });
        }

        if (vm.Name != null)
        {
            existingProduct.Name = vm.Name;
        }

        existingProduct.MemberModifiedId = -1;
        existingProduct.DateTimeModified = DateTime.UtcNow;
        
        _context.Product.Update(existingProduct);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ActionName("productDelete")]
    public IActionResult ProductDelete(long id)
    {
        //if (_currentUser == null) // Toegangscontrole
        //    return Forbidden();

        var existingProduct = _context.Product
            .Where(dbRec => dbRec.Id == id)
            .FirstOrDefault();

        if (existingProduct == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        try
        {
            _context.Product.Remove(existingProduct);
            _context.SaveChanges();
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

    private static bool ProductsAreEqual(Product existingProduct, ProductViewModel vm)
    {
        bool nameEqual = (vm.Name == null)
                         || (existingProduct.Name == vm.Name);

        return nameEqual;
    }
    
}