using System.Diagnostics;
using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Photos;
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

/*
    [HttpPost]
	public IActionResult Nieuw(DeclaratieViewModel viewModel)
	{
        //// TODO: Omzetten naar API (dit dus niet meer gebruiken)
        
		ViewData["Member"] = _context.Member.FirstOrDefault();

        // Form data bewaren zodat we die weer kunnen tonen als er iets mis is
        //ViewData["Amount"] = Amount;
        //ViewData["Note"] = Note;
        //ViewData["Error"] = null;

        // Wat is de eerste status ("Ingediend")? (Als ReceiptStatus nou een Enum was zou je bijv. ReceiptStatus.Submitted kunnen gebruiken)
        var initialStatus = _context.ReceiptStatus.FirstOrDefault(); // Eerste de beste status dan maar

        var receipt = new Receipt() {
            Amount = viewModel.Amount,
            Note = viewModel.Note,
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

        var receiptEntered = _context.Receipt.OrderByDescending(x => x.DateTimeCreated).FirstOrDefault();

        var fileContent = FileHelpers.ProcessFormFile<BufferedSingleFileUploadDb>(
                    viewModel.FormFile, ModelState, new string[] { "png", "txt" },
                    102400000).Result;

        var photo = new Photo()
        {
            Bytes = fileContent,
            DateTimeDeleted = DateTime.UtcNow,
            fileName = viewModel.FormFile.FileName,
            fileExtension = "blaat",
            Receipt = receiptEntered.Id,
            MemberCreatedId = (ViewData["Member"] as Member)?.Id ?? 0,
        };


        try
        {
            _context.Photo.Add(photo);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Exception? error = e;
            string errorMessage = "";
            while (error != null)
            {
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
*/

    public IActionResult Nieuw()
    {
        return View();
    }
}