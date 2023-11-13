using Maasgroep.Database;
using Maasgroep.Database.Members;
using Maasgroep.Database.Receipts;
using Maasgroep.Database.Repository.ViewModel;
using Maasgroep.Database.Repository.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly MaasgroepContext _context;

    public SessionController(MaasgroepContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ActionName("sessionGet")]
    public IActionResult SessionGet()
    {
        // Get current user session (member that is currently logged in)
        // For now, that is the first member in the database, or none if there are no members in the DB
        MemberViewModel? currentUser = _context.Member.Select(dbRec => new MemberViewModel(dbRec)).FirstOrDefault();
        if (currentUser == null)
            return NotFound(new
            {
                status = 404,
                message = "No member found. Please make sure there is at least one member in the database."
            });
        return Ok(currentUser);
    }
    
}