using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TubeGram.API.Helpers;
using TubeGram.API.Models;

namespace TubeGram.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;
        public UserController(ApplicationContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            var newUser = new User(userDto.username, EncryptPassword(userDto.password, "salt"),
                userDto.email);
            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(200);
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] CreateUserDTO newUser)
        {
            User? user;
            try
            {
                user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Username == newUser.username && u.Password == EncryptPassword(newUser.password, "salt"));
                if (user is null)
                {
                    return StatusCode(401);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
            JsonWebTokenHandler handler = new JsonWebTokenHandler();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(int.Parse(_config["Jwt:TokenLifetime"]!)),
                Audience = _config["Jwt:Audience"],
                Issuer = _config["Jwt:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return StatusCode(200, new ResponseUserDto(user.Id, token));
        }

        //TODO: Rewrite to Argon2/BCrypt
        private string EncryptPassword(string password, string salt)
        {
            var _sha512 = SHA512.Create();
            //From String to byte array
            byte[] sourceBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = _sha512.ComputeHash(sourceBytes);
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }
    }

    public record UserDTO(string username, string password, string email);

    public record CreateUserDTO(string username, string password);

    public record ResponseUserDto(int id, string token);
}
