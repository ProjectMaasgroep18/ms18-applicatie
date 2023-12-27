using Maasgroep.Database.Admin;
using Maasgroep.Database.Interfaces;
using Maasgroep.Exceptions;

using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Data.Common;

namespace Maasgroep.Controllers.Api;

public class UserController : EditableRepositoryController<IMemberRepository, Member, MemberModel, MemberData, MemberHistory>
{
    protected readonly IReceiptRepository Receipts;
    protected readonly IBillRepository Bills;
    protected readonly ITokenStoreRepository TokenStore;
    protected readonly IConfiguration Config;
    public override string ItemName { get => "Gebruiker"; }

    public UserController(IMemberRepository repository, IReceiptRepository receipts, IBillRepository bills, ITokenStoreRepository tokenStore, IConfiguration config) : base(repository)
    {
        Receipts = receipts;
        Bills = bills;
        TokenStore = tokenStore;
        Config = config;
    }

    protected override bool AllowList()
        => HasPermission("admin");

    protected override bool AllowView(Member? member)
        => HasPermission("admin") || member?.Id == CurrentMember?.Id;

    protected override bool AllowCreate(MemberData member)
        => HasPermission("admin");

    protected override bool AllowDelete(Member? member)
    {
        if (!HasPermission("admin"))
            return false;
        if (member?.Id == CurrentMember!.Id)
            throw new MaasgroepForbidden("Je kunt niet je eigen account verwijderen");
        return true;
    }

    protected override bool AllowEdit(Member? member, MemberData? newData)
    {
        if (CurrentMember?.Id == null || (member?.Id != CurrentMember.Id && !HasPermission("admin")))
            return false;
        if (newData?.Email != null && newData.Email != "" && newData.Email != member?.Email)
            throw new MaasgroepForbidden("E-mailadres wijzigen kan alleen via Credentials");
        return true;
    }

    [HttpPut("{id}/Credentials")]
    public IActionResult UserChangeCredentials(long id, [FromBody] MemberCredentialsData newCredentials)
    {
        var member = Repository.GetById(id);
        if (member == null || !AllowView(member))
            NotFound();
        if (!AllowEdit(member, null))
            NoAccess();

        if (member?.Id == CurrentMember!.Id && newCredentials.NewPermissions != null)
            throw new MaasgroepForbidden("Je mag niet je eigen rechten aanpassen");

        if (member?.Id == CurrentMember!.Id && member.IsGuest)
            throw new MaasgroepForbidden("Als gast heb je geen toegang tot deze functie");

        // Current user should supply their password to edit credentials
        if (newCredentials.NewPassword != null || newCredentials.NewPermissions != null)
        {
            if ((newCredentials.CurrentPassword ?? "") == "")
                throw new MaasgroepForbidden("Geef je huidige wachtwoord op om een wachtwoord of gebruikersrechten aan te passen");
            if (!Repository.CheckPassword(newCredentials.CurrentPassword!, CurrentMember.Id))
                throw new MaasgroepForbidden("Huidige wachtwoord is niet juist");
        }
        
        if (Repository.Update(member!, newCredentials, CurrentMember?.Id) == null)
            throw new MaasgroepBadRequest($"{ItemName} kon niet worden opgeslagen");
        return NoContent();
    }

    [HttpGet("{id}/Receipt")]
    public IActionResult UserGetReceipts(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {   
        var requiredPermission = id == CurrentMember?.Id ? "receipt" : "receipt.approve";
        if (!HasPermission(requiredPermission))
            NoAccess();
        return Ok(Receipts.ListByMember(id, offset, limit, includeDeleted));
    }

    [HttpGet("{id}/Bill")]
    public IActionResult UserGetBills(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {   
        if (id != CurrentMember?.Id && !HasPermission("order.view"))
            NoAccess();
        return Ok(Bills.ListByMember(id, offset, limit, includeDeleted));
    }

    [HttpGet("Bill")]
    public IActionResult BillsByEmail([FromQuery] string email, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
    {   
        if (!HasPermission("order.view"))
            NoAccess();
        return Ok(Bills.ListByEmail(email, offset, limit, includeDeleted));
    }

    [HttpGet("{id}/BillTotal")]
    public IActionResult UserGetBillTotal(long id)
    {   
        if (id != CurrentMember?.Id && !HasPermission("order.view"))
            NoAccess();
        return Ok(Bills.GetTotal(id));
    }

    [HttpGet("Current")]
    public IActionResult CurrentUser()
        => Ok(CurrentMember ?? throw new MaasgroepUnauthorized());

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginData data)
    {
        data.Password ??= "";
        var user = Repository.GetByEmail(data.Email, data.Password) ?? throw new MaasgroepUnauthorized("E-mailadres of wachtwoord is niet juist");
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"] ?? ""));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expire = DateTime.UtcNow.AddDays(90);
        var token = new JwtSecurityToken(Config["Jwt:Issuer"], Config["Jwt:Issuer"], null, expires: expire, signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        TokenStore.SaveToken(tokenString, expire, user.Id);

        return Ok(new TokenModel() {
            Member = user,
            Token = tokenString,
            ExpirationDate = expire,
        });
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        if (HttpContext.Items["Token"] is string token)
            TokenStore.DeleteToken(token, CurrentMember?.Id ?? 0);
        return NoContent();
    }
}