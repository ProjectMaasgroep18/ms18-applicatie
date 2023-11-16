using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly MaasgroepContext _context;

    public UserController(MaasgroepContext context)
    {
        _context = context;
    }

    [HttpGet("current")]
    [ActionName("userGetCurrent")]
    public IActionResult UserGetCurrent()
    {
        // Get current user session (member that is currently logged in)
        // For now, that is the first member in the database, or none if there are no members in the DB
        MemberViewModel? currentUser = _context.Member.OrderBy(dbRec => dbRec.Id).Select(dbRec => new MemberViewModel(dbRec)).FirstOrDefault();
        if (currentUser == null)
            return NotFound(new
            {
                status = 404,
                message = "No member found. Please make sure there is at least one member in the database."
            });
        return Ok(currentUser);
    }

    [HttpGet("{id}")]
    [ActionName("userGetById")]
    public IActionResult UserGetById(long id)
    {
        // Get user by ID
        Member? user = _context.Member.Find(id);
        if (user == null)
            return NotFound(new
            {
                status = 404,
                message = "User not found"
            });
        return Ok(new MemberViewModel(user));
    }

    [HttpGet("{id}/Receipt")]
    public IActionResult UserGetReceipts(long id)
    {
        
        // Get user by ID
        Member? user = _context.Member.Find(id);
        
        // Check if the receipt with the provided ID exists
        if (user == null)
        {
            return NotFound(new
            {
                status = 404,
                message = "User not found"
            });
        }
        
        // Get all receipts for the user
        var receipts = _context.Receipt
            .Where(receipt => receipt.MemberCreatedId == user.Id)
            .Select(receipt => new ReceiptViewModel(receipt))
            .ToList();
        
        return Ok(receipts);
    }
}