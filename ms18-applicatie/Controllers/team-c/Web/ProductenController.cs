using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers;

public class ProductenController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Bestellen()
    {
        return View();
    }

    public IActionResult Producten()
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

    public IActionResult Bestellingen()
    {
        return View();
    }

    public IActionResult AlleBestellingen()
    {
        return View();
    }
}