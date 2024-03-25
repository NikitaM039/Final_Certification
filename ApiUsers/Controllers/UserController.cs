using ApiUsers.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiLibr.Models.DTO;
using WebApiLibr.rsa;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public UserController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("add_user_or_login")]
        public IActionResult Login([FromBody] LoginModel userDTO)
        {
            var entityUser = _userRepository.AddUser(userDTO);

            if (entityUser != null)
            {
                var token = GenerateToken(entityUser);
                return Ok(token);
            }
            return NotFound("Пользователь с данным логином использует другой пароль)");
        }

        [HttpGet]
        [Route("get_users")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }


        [HttpDelete]
        [Route("delete_users_to_admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string user)
        {
            var result = _userRepository.DeleteUser(user);
            return Ok(result);
        }


        private string GenerateToken(UserDTO user)
        {
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var key = new RsaSecurityKey(RSATools.GetPrivateKey());

            var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claim,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
