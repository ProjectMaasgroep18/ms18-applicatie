using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;
public abstract class BaseController : ControllerBase
{
    // Base controller for all other controllers, provides access to dbContext and to the currently logged-in User (if any)

    protected readonly MaasgroepContext _context;
    protected Member? _currentUser;

    public BaseController(MaasgroepContext context)
    {
        _context = context;
        _currentUser = context.Member.OrderBy(user => user.Id).FirstOrDefault();
        // Todo: This should be the user that is actually logged in, using e.g. a token from Request.Headers
    }

    [NonAction]
    public IActionResult Forbidden(string? message = null)
    {
        return Forbid(message ?? (_currentUser == null ? "Je bent niet ingelogd" : "Je hebt geen toegang tot deze functie"));
    }

    [NonAction]
    public ReceiptViewModel AddForeignData(ReceiptViewModel receipt)
    {
        // Add data from foreign database tables to the ReceiptViewModel
        // Photos, Status, CostCentre, etc.

        receipt.Status = "TODO ENUM";
        
        receipt.ReceiptPhotoURI = _context.Photo.Where(photo => photo.ReceiptId == receipt.Id).Select(receipt => $"/api/v1/ReceiptPhoto/{receipt.Id}").ToList();
        if (receipt.CostCentreId != null) {
            var costCentre = _context.CostCentre.FirstOrDefault(costCentre => costCentre.Id == receipt.CostCentreId);
            receipt.CostCentreURI = "/api/v1/CostCentre/" + costCentre?.Id;
            receipt.CostCentre = costCentre?.Name;
            System.Console.WriteLine(receipt.CostCentreURI + " -> " + receipt.CostCentre);
        }
        
        return receipt;
    }

    [NonAction]
    public ProductViewModel AddForeignData(ProductViewModel product)
    {
        product.StockpileURI = "/api/v1/Stockpile/" + product.Id;
        
        return product;
    }
}