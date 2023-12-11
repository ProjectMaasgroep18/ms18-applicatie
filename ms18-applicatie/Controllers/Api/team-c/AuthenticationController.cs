using Maasgroep.Database;
using Maasgroep.Database.Members;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ms18_applicatie.Controllers.Api.team_c
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        protected readonly MaasgroepContext _context;
        private IConfiguration _config;
        //protected Member? _user;

        public AuthenticationController(MaasgroepContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public IActionResult ValidateUser(Member user)
        {
            IActionResult response = Unauthorized();

            if (user != null)
            {
                bool validUser = _context.Member.Any(x => x.Id == user.Id);
                
                if (validUser)
                {
                    var token = GenerateJSONWebToken(user);
                    response = Ok(token);

                    //Implement DB insert
                }
            }

            return response;
        }

        private string GenerateJSONWebToken(Member user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime experationTime = DateTime.Now.AddMinutes(120);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: experationTime,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
