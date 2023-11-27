using Microsoft.AspNetCore.Mvc;
using Ms18.Database;
using Ms18.Database.Models.TeamC.Stock;
using Ms18.Database.Repository.TeamC.ViewModel;

namespace Ms18.Application.Controllers.TeamC;
[Route("api/v1/[controller]")]
[ApiController]
public class StockController : BaseController
{
    public StockController(MaasgroepContext context) : base(context)
    {
    }

    [HttpGet("{productId}")]
    [ActionName("stockGet")]
    public IActionResult StockGet(long productId)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();

        var dbRecProduct = _context.Product
            .FirstOrDefault(dbRec => dbRec.Id == productId);

        if (dbRecProduct == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        var dbRec = _context.Stock
            .FirstOrDefault(dbRec => dbRec.ProductId == productId);

        if (dbRec == null)
        {
            // If there is no stock for this product, create empty stock
            dbRec = new Stockpile
            {
                Product = dbRecProduct,
                Quantity = 0,
                MemberCreatedId = _currentUser.Id
            };


            // Try and add the stock to the database and return it
            try
            {
                _context.Stock.Add(dbRec);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = 422,
                    message = "Kon voorraad niet aanmaken"
                });
            }
        }

        return Ok(new StockpileViewModel(dbRec));
    }

    [HttpPut("{productId}/Increase")]
    [ActionName("stockIncrease")]
    public IActionResult StockIncrease(long productId, [FromBody] StockQuantityViewModel quantityModel)
    {
        if (_currentUser == null) // Toegangscontrole
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

        var dbRecProduct = _context.Product
            .FirstOrDefault(dbRec => dbRec.Id == productId);

        if (dbRecProduct == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        var dbRec = _context.Stock
            .FirstOrDefault(dbRec => dbRec.ProductId == productId);

        if (dbRec == null)
        {
            // If there is no stock for this product, create empty stock
            dbRec = new Stockpile
            {
                Product = dbRecProduct,
                Quantity = quantityModel.Quantity,
                MemberCreatedId = _currentUser.Id
            };

            // Try and add the stock to the database and return it
            try
            {
                _context.Stock.Add(dbRec);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "Kon voorraad niet aanmaken"
                });
            }
        }
        else
        {
            dbRec.Quantity += quantityModel.Quantity;


            // Try and add the stock to the database and return it
            try
            {
                _context.Stock.Update(dbRec);
                _context.SaveChanges();
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
    [ActionName("stockDecrease")]
    public IActionResult StockDecrease(long productId, [FromBody] StockQuantityViewModel quantityModel)
    {
        if (_currentUser == null) // Toegangscontrole
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

        var dbRecProduct = _context.Product
            .FirstOrDefault(dbRec => dbRec.Id == productId);

        if (dbRecProduct == null)
            return NotFound(new
            {
                status = 404,
                message = "Product niet gevonden"
            });

        var dbRec = _context.Stock
            .FirstOrDefault(dbRec => dbRec.ProductId == productId);

        if (dbRec == null)
        {
            // If there is no stock for this product, create empty stock
            dbRec = new Stockpile
            {
                Product = dbRecProduct,
                Quantity = quantityModel.Quantity,
                MemberCreatedId = _currentUser.Id
            };

            // Try and add the stock to the database and return it
            try
            {
                _context.Stock.Add(dbRec);
                _context.SaveChanges();
                
                return UnprocessableEntity(new
                {
                    status = 422,
                    message = "Kon voorraad niet wijzigen"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "Kon voorraad niet aanmaken"
                });
            }
        }
        else
        {
            
            if(dbRec.Quantity < quantityModel.Quantity)
                return UnprocessableEntity(new
                {
                    status = 422,
                    message = "Kon voorraad niet wijzigen"
                });
            
            dbRec.Quantity -= quantityModel.Quantity;


            // Try and add the stock to the database and return it
            try
            {
                _context.Stock.Update(dbRec);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = 422,
                    message = "Kon voorraad niet wijzigen"
                });
            }
        }

        return NoContent();
    }

    //
    // [HttpPost]
    // [ActionName("productCreate")]
    // public IActionResult ProductCreate([FromBody] ProductViewModel vm)
    // {
    //     
    //     if (_currentUser == null) // Toegangscontrole
    //         return Forbidden();
    //
    //     // Validate the request body
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(new
    //         {
    //             status = 400,
    //             message = "Invalid request body"
    //         });
    //     }
    //     
    //     var createdProduct = new Product
    //     {
    //         Name = vm.Name,
    //         MemberCreatedId = _currentUser.Id
    //     };
    //
    //     try
    //     {
    //         _context.Product.Add(createdProduct);
    //         _context.SaveChanges();
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(new
    //         {
    //             status = 422,
    //             message = "Kon product niet aanmaken",
    //         });
    //     }
    //
    //     return Ok(new
    //     {
    //         status = 200,
    //         message = "Product aangemaakt",
    //         product = new ProductViewModel(createdProduct)
    //     });
    // }
    //
    // [HttpPut("{id}")]
    // [ActionName("productUpdate")]
    // public IActionResult ProductUpdate(long id, [FromBody] ProductViewModel vm)
    // {
    //     if (_currentUser == null) // Toegangscontrole
    //         return Forbidden();
    //
    //     // Validate the request body
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(new
    //         {
    //             status = 400,
    //             message = "Invalid request body"
    //         });
    //     }
    //     
    //     if (id != vm.Id)
    //         return BadRequest(new
    //         {
    //             status = 400,
    //             message = "Invalid request body, ID in URL does not match ID in request body"
    //         });
    //
    //     var existingProduct = _context.Product
    //         .Where(dbRec => dbRec.Id == id)
    //         .FirstOrDefault();
    //
    //     if (existingProduct == null)
    //     {
    //         return NotFound(new
    //         {
    //             status = 404,
    //             message = "Product niet gevonden"
    //         });
    //     }
    //
    //     if (ProductsAreEqual(existingProduct, vm))
    //     {
    //         // If the data is the same, return a response indicating no update was performed
    //         return Ok(new {
    //             status = 200,
    //             message = "Product niet gewijzigd"
    //         });
    //     }
    //
    //     if (vm.Name != null)
    //     {
    //         existingProduct.Name = vm.Name;
    //     }
    //
    //     existingProduct.MemberModifiedId = _currentUser.Id;
    //     existingProduct.DateTimeModified = DateTime.UtcNow;
    //     
    //     _context.Product.Update(existingProduct);
    //     _context.SaveChanges();
    //
    //     return NoContent();
    // }
    //
    // [HttpDelete("{id}")]
    // [ActionName("productDelete")]
    // public IActionResult ProductDelete(long id)
    // {
    //     if (_currentUser == null) // Toegangscontrole
    //         return Forbidden();
    //
    //     var existingProduct = _context.Product
    //         .Where(dbRec => dbRec.Id == id)
    //         .FirstOrDefault();
    //
    //     if (existingProduct == null)
    //         return NotFound(new
    //         {
    //             status = 404,
    //             message = "Product niet gevonden"
    //         });
    //
    //     try
    //     {
    //         _context.Product.Remove(existingProduct);
    //         _context.SaveChanges();
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(new
    //         {
    //             status = 409,
    //             message = "Kon product niet verwijderen",
    //         });
    //     }
    //
    //     return NoContent();
    // }
    //
    // private static bool ProductsAreEqual(Product existingProduct, ProductViewModel vm)
    // {
    //     bool nameEqual = (vm.Name == null)
    //                      || (existingProduct.Name == vm.Name);
    //
    //     return nameEqual;
    // }
}