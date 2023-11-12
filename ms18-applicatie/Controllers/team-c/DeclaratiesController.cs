using System.Diagnostics;
using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Microsoft.AspNetCore.Mvc;


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

    [HttpPost]
	public IActionResult Nieuw(decimal Amount, string Note)
	{
        //// TODO: Omzetten naar API (dit dus niet meer gebruiken)
        
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
            MemberCreatedId = (ViewData["Member"] as Member)?.Id ?? 0,
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
        // ViewData["Member"] = _context.Member.FirstOrDefault();
        return View();
    }
}