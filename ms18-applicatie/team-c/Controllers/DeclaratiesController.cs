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
        ViewData["Receipts"] = _context.Receipt.Where(r => r.UserCreatedId == (ViewData["Member"] as Member).Id).ToArray();
        return View();
    }

    [HttpPost]
	public IActionResult Nieuw(decimal Amount, string Note)
	{
		ViewData["Member"] = _context.Member.FirstOrDefault();

        // Form data bewaren zodat we die weer kunnen tonen als er iets mis is
        ViewData["Amount"] = Amount;
        ViewData["Note"] = Note;
        ViewData["Error"] = null;

        // Wat is de eerste status ("Ingediend")? (Als ReceiptStatus nou een Enum was zou je bijv. ReceiptStatus.Submitted kunnen gebruiken)
        var initialStatus = _context.ReceiptStatus.FirstOrDefault(); // Eerste de beste status dan maar

        var receipt = new Receipt() {
            Amount = Amount,
            Note = Note,
            ReceiptStatusId = initialStatus?.Id ?? 0,
            UserCreatedId = (ViewData["Member"] as Member)?.Id ?? 0,
        };

        try {
            // Hier kunnen validatiezaken komen
            _context.Receipt.Add(receipt);
            _context.SaveChanges();
        } catch (Exception e) {
            Exception? error = e;
            string errorMessage = "";
            while (error != null) {
                // Zoek de "binnenste" (nuttigste) exception
                errorMessage = error.Message;
                error = error.InnerException; 
            }
            ViewData["Error"] = errorMessage;
        }

        // Als het gelukt is
        if (ViewData["Error"] == null)
            return RedirectToAction(nameof(Index));

        // Als er een validatie-error is
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