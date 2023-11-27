using Microsoft.AspNetCore.Mvc;
using Ms18.Database;
using Ms18.Database.Models.TeamC.Admin;
using Ms18.Database.Repository.TeamC.ViewModel;

namespace Ms18.Application.Controllers.TeamC;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : BaseController
{
    public UserController(MaasgroepContext context) : base(context) { }

    [HttpGet("Current")]
    [ActionName("userGetCurrent")]
    public IActionResult UserGetCurrent()
    {
        // Get current user session (member that is currently logged in)
        // For now, that is the first member in the database, or none if there are no members in the DB (see BaseController)
        if (_currentUser == null)
            return NotFound(new
            {
                status = 404,
                message = "No member found. Please make sure there is at least one member in the database."
            });
        return Ok(new MemberViewModel(_currentUser));
    }

    [HttpGet("{id}")]
    [ActionName("userGetById")]
    public IActionResult UserGetById(long id)
    {
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();

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
        if (_currentUser == null) // Toegangscontrole
            return Forbidden();
        
        // Get user by ID
        Member? user = _context.Member.Find(id);
        
        // Check if the user with the provided ID exists
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
        
        receipts.ForEach(receipt => AddForeignData(receipt));
        
        return Ok(receipts);
    }
}