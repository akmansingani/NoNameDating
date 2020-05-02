using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.API.Data;
using Project.API.Dtos;
using Project.API.Models;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _authrepo;

        private readonly IConfiguration _config;

        private readonly IMapper _mapper;
        public AuthController(IAuthRepo authrepo,IConfiguration config,IMapper mapper)
        {
            _authrepo = authrepo;
            _config = config;
            _mapper = mapper;
        }
        // GET: api/Auth
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST: api/Auth
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto user)
        {
            if(await _authrepo.UserExists(user.Username.ToLower()))
            {
                return BadRequest("Username already exists!");
            }

            var userModel = _mapper.Map<Users>(user);

            var result = await _authrepo.Register(userModel, user.Password);

            var returnUser = _mapper.Map<UserListDto>(result);

            return CreatedAtRoute("GetUser",new { controller = "User", id = userModel.UserID }, returnUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            var result = await _authrepo.Login(user.Username.ToLower(), user.Password);
            
            if(result == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserID.ToString()),
                new Claim(ClaimTypes.Name, result.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII
               .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            result.ActiveDate = DateTime.Now;
            _authrepo.SaveAllChanges();

            var userResponse = _mapper.Map<UserLoginResponseDto>(result);
            
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user = userResponse
            }); 
        }


    }
}
