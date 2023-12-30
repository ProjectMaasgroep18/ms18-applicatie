using Maasgroep.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Maasgroep.Controllers.Web;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Profiel()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        throw new MaasgroepBadRequest("Er is een onbekende fout opgetreden");
    }
}