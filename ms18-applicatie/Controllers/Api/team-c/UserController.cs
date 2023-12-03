using Maasgroep.SharedKernel.Interfaces.Members;
using Maasgroep.SharedKernel.Interfaces.Receipts;
using Microsoft.AspNetCore.Mvc;
using ms18_applicatie.Services;

namespace ms18_applicatie.Controllers.Api;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly IMemberRepository _memberRepository;
    private readonly IMemberService _memberService;
    private readonly IReceiptRepository _receiptRepository;

	public UserController(IMemberRepository memberRepository, IMemberService memberService, IReceiptRepository receiptRepository)
	{
		_memberRepository = memberRepository;
        _memberService = memberService;
        _receiptRepository = receiptRepository;
	}

	[HttpGet("Current")]
    [ActionName("userGetCurrent")]
    public IActionResult UserGetCurrent()
    {
        // Get current user session (member that is currently logged in)
        // For now, that is the first member in the database, or none if there are no members in the DB (see BaseController)
        if (!MemberExists(1))
            return NotFound(new
            {
                status = 404,
                message = "No member found. Please make sure there is at least one member in the database."
            });

        return Ok(_memberRepository.GetMember(1));
    }

    [HttpGet("{id}")]
    [ActionName("userGetById")]
    public IActionResult UserGetById(long id)
    {
        if (!MemberExists(1)) // Toegangscontrole
            return Forbidden();

        // Get user by ID
        if (!MemberExists(id))
            return NotFound(new
            {
                status = 404,
                message = "User not found"
            });

        return Ok(_memberRepository.GetMember(id));
    }

    //TODO: KH, ik zou dit onder receipt plaatsen, niet member (we doen hier niks met memberrepositry)
    [HttpGet("{id}/Receipt")]
    public IActionResult UserGetReceipts(long id)
    {
		if (!MemberExists(1)) // Toegangscontrole
			return Forbidden();

		// Check if the user with the provided ID exists
		if (!MemberExists(1))
        {
            return NotFound(new
            {
                status = 404,
                message = "User not found"
            });
        }

        // Get all receipts for the user
        var receipts = _receiptRepository.GetReceiptsByMember(id, 0, Int32.MaxValue);
        
        return Ok(receipts);
    }

	private bool MemberExists(long id) => _memberService.MemberExists(id);
	private IActionResult Forbidden() => Forbid("Je hebt geen toegang tot deze functie");
}