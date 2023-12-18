using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers;

public class LedenbeheerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}