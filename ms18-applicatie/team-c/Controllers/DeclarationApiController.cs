using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Database;
using ms18_applicatie.Models;

namespace ms18_applicatie.Controllers;

public class DeclarationApiController : Controller
{
    private readonly ILogger<DeclaratiesController> _logger;
    private readonly MaasgroepContext _context;

    public DeclarationApiController(MaasgroepContext context, ILogger<DeclaratiesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {

        var db = new MaasgroepContext();

        var allReceipts = db.Receipt.ToList();
        
        return Ok(allReceipts);
    }
}