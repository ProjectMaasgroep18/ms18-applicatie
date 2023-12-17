using Maasgroep.Database.Admin;
using Maasgroep.Database.Interfaces;
using Maasgroep.Exceptions;

using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.DataModels.Admin;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Maasgroep.Controllers.Api;

public class UserController : DeletableRepositoryController<IMemberRepository, Member, MemberModel, MemberData>
{
    protected readonly IReceiptRepository Receipts;
    protected readonly ITokenStoreRepository TokenStore;
    protected readonly IConfiguration Config;

    public UserController(IMemberRepository repository, IReceiptRepository receipts, ITokenStoreRepository tokenStore, IConfiguration config) : base(repository)
    {
        Receipts = receipts;
        TokenStore = tokenStore;
        Config = config;
    }

    [HttpGet("{id}/Receipt")]
    public IActionResult UserGetReceipts(long id, [FromQuery] int offset = default, [FromQuery] int limit = default, [FromQuery] bool includeDeleted = default)
        => Ok(Receipts.ListByMember(id, offset, limit, includeDeleted));

    [HttpGet("Current")]
    public IActionResult CurrentUser()
        => Ok(CurrentMember ?? throw new MaasgroepUnauthorized());

    [HttpGet("Login")]
    public IActionResult Login(string email, string password)
    {
        var user = Repository.GetByEmail(email, password) ?? throw new MaasgroepUnauthorized("E-mailadres of wachtwoord is niet juist");
        
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
}