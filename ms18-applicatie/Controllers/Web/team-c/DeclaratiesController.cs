using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers;

public class DeclaratiesController : Controller
{
    public DeclaratiesController()
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Lijst()
    {
        return View();
    }

    public IActionResult Aanpassen(long id)
    {
        ViewData["id"] = id;
        return View();
    }

    public IActionResult Nieuw()
    {
        return View();
    }

    public IActionResult Kostencentra()
    {
        return View();
    }
}