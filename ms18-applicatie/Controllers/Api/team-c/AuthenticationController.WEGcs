using Maasgroep.SharedKernel.Interfaces.Token;
using Maasgroep.SharedKernel.ViewModels.Admin;
using Maasgroep.SharedKernel.ViewModels.team_c.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ms18_applicatie.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ms18_applicatie.Controllers.Api.team_c
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenStoreRepository _tokenRepository;
        private readonly IMemberService _memberService;
        private IConfiguration _config;

        public AuthenticationController(ITokenStoreRepository tokenRepository, IMemberService memberService, IConfiguration config)
        {
            _tokenRepository = tokenRepository;
            _memberService = memberService;
            _config = config;
        }

        [HttpPost]
        [ActionName("login")]
        public IActionResult ValidateUser(string email, string password)
        {
            IActionResult response = Unauthorized();

            //bool validUser = _context.Member.Any(x => x.Id == user.Id && x.Password == user.Password);
            var user = _memberService.GetMemberByEmail(email);

            if (user != null && user.Password == password)
            {
                var registeredToken = _tokenRepository.RetrieveToken(user.Id);
                if (registeredToken != null)
                {
                    var token = GenerateJSONWebToken(user);
                    response = Ok(token);

                    //Implement DB insert
                    TokenModelUpdate newToken = new TokenModelUpdate
                    {
                        Token = token,
                        ExpirationDate = DateTime.UtcNow.AddDays(90),
                    };

                    _tokenRepository.UpdateUserToken(newToken, user.Id);                    
                    
                }
                else
                {
                    var token = GenerateJSONWebToken(user);
                    response = Ok(token);

                    //Implement DB insert
                    TokenModelCreate newToken = new TokenModelCreate
                    {
                        Token = token,
                        ExpirationDate = DateTime.UtcNow.AddDays(90),
                        MemberId = user.Id
                    };

                    _tokenRepository.SaveToken(newToken);
                }
            }

            return response;
        }

        [HttpGet]
        [ActionName("getToken")]
        public IActionResult GetUserToken(long userId)
        {
            var member = _memberService.GetMember(userId);

            if (member == null) return BadRequest("Member niet gevonden");

            var token = _tokenRepository.RetrieveToken(userId);

            if (token == null) return BadRequest("Deze user heeft geen token");

            return Ok(token);
        }

        private string GenerateJSONWebToken(MemberModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.UtcNow.AddDays(90),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
