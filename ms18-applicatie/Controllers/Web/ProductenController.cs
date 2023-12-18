using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers;

public class ProductenController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}