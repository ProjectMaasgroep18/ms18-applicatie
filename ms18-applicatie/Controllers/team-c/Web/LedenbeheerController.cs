using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers;

public class LedenbeheerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Nieuw()
    {
        return View();
    }

    public IActionResult Aanpassen(long id)
    {
        ViewData["id"] = id;
        return View();
    }
}