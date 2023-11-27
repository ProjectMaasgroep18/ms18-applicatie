using System.Diagnostics;
using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Receipts;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Models;
using ms18_applicatie.Utilities;

namespace ms18_applicatie.Controllers;

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