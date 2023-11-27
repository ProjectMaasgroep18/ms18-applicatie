using Microsoft.AspNetCore.Mvc;
using Ms18.Database;

namespace Ms18.Application.Controllers;

public class DeclaratiesController : Controller
{
    private readonly MaasgroepContext _context;

    public DeclaratiesController(MaasgroepContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Edit(long id)
    {
        ViewData["id"] = id;
        return View();
    }

    public IActionResult Nieuw()
    {
        return View();
    }
}