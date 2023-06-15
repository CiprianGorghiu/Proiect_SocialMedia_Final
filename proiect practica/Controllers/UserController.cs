using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using proiect_practica.Data;
using proiect_practica.Models;
using proiect_practica.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace proiect_practica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DBcontext db;
        private readonly IConfiguration config;

        public UserController(DBcontext db, IConfiguration config)
        {

            this.db = db;
            this.config = config;
        }
        [HttpPost("Register")]

        public IActionResult Register([FromBody] RegisterUserDTO user)
        {
            try
            {
                if (db.Users.Any(x => x.Email == user.Email))
                {
                    throw new Exception();
                }
                byte[] passwordHash, passwordKey;
                using (var hmac = new HMACSHA512())
                {
                    passwordKey = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));

                }
                User account = new User
                {
                    Email = user.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordKey,
                    Name = user.Name,
                };
                db.Users.Add(account);
                db.SaveChanges();
                return Ok();
            }
            catch(Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, "Eroare, ceva nu a mers bine!"); }
           
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginUserDTO user)
        {
            try
            {
                var userInfo = db.Users.SingleOrDefault(x => x.Email.Equals(user.Email));
                if (userInfo == null || userInfo.PasswordSalt == null)
                {
                    return BadRequest("Emailul nu se regaseste in baza de date!");
                }
                if (!MatchPasswordHash(user.Password, userInfo.PasswordHash, userInfo.PasswordSalt))
                {
                    return BadRequest("Parola gresita!");

                }
                return Ok(CreateJWT(userInfo));
                 }
            catch(Exception e) { return StatusCode(StatusCodes.Status500InternalServerError, "Eroare, ceva nu a mers bine!"); }
        }
        private bool MatchPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using(var hmac = new HMACSHA512(salt))
            {
                var passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i < hash.Length; i++)
                {
                    if (hash[i] != passwordhash[i])
                    {
                        return false;
                    }

                }
                return true;
            }
        }
        private string CreateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var token = new JwtSecurityToken(config["Jwt:Issuer"],
               config["Jwt:Audience"],
               claims,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
