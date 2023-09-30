using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Database;
using ms18_applicatie.Models;

namespace ms18_applicatie.Controllers;

public class DeclaratiesController : Controller
{
    private readonly ILogger<DeclaratiesController> _logger;
    private readonly MaasgroepContext _context;

    public DeclaratiesController(MaasgroepContext context, ILogger<DeclaratiesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Member"] = _context.Member.FirstOrDefault();
        return View();
    }

     public IActionResult Nieuw()
    {
        ViewData["Member"] = _context.Member.FirstOrDefault();
        return View();
    }

    // public IActionResult Privacy()
    // {
    //     return View();
    // }
}