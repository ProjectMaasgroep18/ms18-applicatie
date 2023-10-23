using System.Diagnostics;
using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Microsoft.AspNetCore.Mvc;


namespace ms18_applicatie.Controllers;

public class DeclaratiesController : Controller
{
    private readonly ILogger<DeclaratiesController> _logger;
    private readonly MemberRepository _memberRepository;
    private readonly ReceiptRepository _receiptRepository;
    private readonly MaasgroepContext _context;

    public DeclaratiesController(MaasgroepContext context, MemberRepository memberRepository, ReceiptRepository receiptRepository, ILogger<DeclaratiesController> logger)
    {
        _memberRepository = memberRepository;
        _receiptRepository = receiptRepository;
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        
        ViewData["Member"] = _context.Member.FirstOrDefault();
        ViewData["Receipts"] = _context.Receipt.Where(r => r.MemberCreatedId == (ViewData["Member"] as Member).Id).ToArray();
        ViewData["ReceiptStatuses"] = _context.ReceiptStatus.ToDictionary(status => status.Id);
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
        ViewData["Member"] = _context.Member.FirstOrDefault();
        return View();
    }

    // public IActionResult Privacy()
    // {
    //     return View();
    // }
    
    //get-routes below should provide JSON response from server, triggered by HTTP request:
    
    // One for the /Receipt route:
    [HttpGet]
    [Route("/Receipt")]
    //(content root path = 
    ///Users/tedruigrok/Documents/ms18_project_c_DEC/ms18-applicatie/ms18-applicatie/)
    public IActionResult GetMemberById(int memberId)
    {
        //retrieve data for this member ID:
        var receiptData = _context.Receipt.FirstOrDefault(_ => _.Id == memberId);

        if (receiptData == null)
        {
            //404 not found exception:
            return NotFound();
        }
        //return data as JSON:
        return Json(receiptData);

    }
}