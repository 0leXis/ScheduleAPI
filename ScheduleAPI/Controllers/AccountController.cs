using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScheduleAPI.Interfaces;
using ScheduleAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAPI.Controllers
{
    public class AccountController : Controller
    {
        private ScheduleContext _db;
        private string _issuer;
        private string _audience;
        private TimeSpan _lifetime;
        private SymmetricSecurityKey _issuerSigningKey;
        private string _globalSalt;
        public AccountController(ScheduleContext db, IConfiguration configuration)
        {
            _db = db;
            _issuer = configuration.GetSection("Jwt").GetValue<string>("Issuer");
            _audience = configuration.GetSection("Jwt").GetValue<string>("Audience");
            _lifetime = TimeSpan.FromMinutes(configuration.GetSection("Jwt").GetValue<int>("Lifetime"));
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Jwt").GetValue<string>("SecretKey")));
            _globalSalt = configuration.GetSection("Password").GetValue<string>("GlobalSalt");
        }

        [HttpPost("account/gettoken")]
        public IActionResult Token([FromBody] LoginModel model, [FromServices] IPasswordService passwordService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var identity = GetIdentity(model.Login, model.Password, passwordService);
            if (identity == null)
                return BadRequest(new { ErrorText = "Invalid username or password." });

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(_lifetime),
                signingCredentials: new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }

        [HttpPost("account/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model, [FromServices] IPasswordService passwordService)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            User user = _db.Users.FirstOrDefault(x => x.Login == model.Login);
            if (user == null)
            {
                var salt = passwordService.GenerateSalt();
                _db.Users.Add(new User()
                {
                    Login = model.Login,
                    Password = passwordService.GetHash(model.Password, salt, _globalSalt),
                    Salt = salt,
                });
                await _db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("User alredy exists");
        }

        private ClaimsIdentity GetIdentity(string username, string password, IPasswordService passwordService)
        {
            User user = _db.Users.FirstOrDefault(x => x.Login == username);

            if (user != null &&
                user.Password == passwordService.GetHash(password, user.Salt, _globalSalt))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}
