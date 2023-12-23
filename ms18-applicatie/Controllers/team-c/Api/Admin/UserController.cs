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
using Maasgroep.Interfaces;

namespace Maasgroep.Controllers.Api;

public class UserController : EditableRepositoryController<IMemberRepository, Member, MemberModel, MemberData, MemberHistory>
{
    protected readonly IReceiptRepository Receipts;
    protected readonly ITokenStoreRepository TokenStore;
    protected readonly IConfiguration Config;

    public UserController(IMemberRepository repository, IReceiptRepository receipts, ITokenStoreRepository tokenStore, IConfiguration config, IMaasgroepAuthenticationService maasgroepAuthenticationService) : base(repository, maasgroepAuthenticationService)
    {
        Receipts = receipts;
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

    protected override bool AllowEdit(Member? member, MemberData newData)
        => CurrentMember?.Id != null && (member?.Id == CurrentMember.Id || HasPermission("admin"));

    [HttpPut("{id}/Credentials")]
    public IActionResult UserChangeCredentials(long id, [FromBody] MemberCredentialsData newCredentials)
    {
        var member = Repository.GetById(id);
        if (member == null || !AllowView(member))
            NotFound();
        if (!AllowEdit(member!, new()))
            NoAccess();

        if (member?.Id == CurrentMember!.Id && newCredentials.NewPermissions != null)
            throw new MaasgroepForbidden("Je mag niet je eigen rechten aanpassen");

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

    [HttpGet("Current")]
    public IActionResult CurrentUser()
        => Ok(CurrentMember ?? throw new MaasgroepUnauthorized());

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginData data)
    {
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