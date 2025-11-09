using WebApplication3.DB;
using WebApplication3.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtSample_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ItCompany1135Context db;

        public AuthController(ItCompany1135Context db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult Login(LoginData data)
        {
            var client = db.Clients.FirstOrDefault(s => s.Login == data.Login
                && s.Password == data.Password);

            if (client == null)
                return Forbid();

            var claims = new List<Claim> {
                new Claim(ClaimValueTypes.Sid, client.Sid),
            };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(token);
        }
    }
}
//КАФТАМЭ