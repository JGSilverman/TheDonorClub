using AutoMapper;
using AutoMapper.Configuration;
using Donator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Donator.Dtos.User;

namespace Donator.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public AuthController(
            IMapper mapper,
            UserManager<User> userManager,
            Microsoft.Extensions.Configuration.IConfiguration config
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("join")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Join(UserToCreateOrgDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Email))
                return Problem("User email address is missing!", null, 404);

            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user != null)
            {
                var userSigninResult = await _userManager.CheckPasswordAsync(user, userDto.Password);
                if (userSigninResult)
                    return CreatedAtRoute("GetUserRoute", new { id = user.Id },
                        new { Status = StatusCode(201), User = _mapper.Map<UserAuthDto>(user), token = GenerateJwt(user) });
                else
                    return Problem("Username already axists, please check your password!", null, 500);
            }

            user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                JobRole = userDto.JobRole,
                Email = userDto.Email,
                UserName = userDto.Email,
            };

            var userCreateResult = await _userManager.CreateAsync(user, userDto.Password);
            if (userCreateResult.Succeeded)
            {
                var userToReturn = _mapper.Map<UserAuthDto>(user);
                return CreatedAtRoute("GetUserRoute", new { id = user.Id },
                        new { Status = StatusCode(201), User = userToReturn, token = GenerateJwt(user) });
            }

            return Problem(userCreateResult.Errors.First().Description, null, 500);
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUp(UserToRegisterDto userToRegister)
        {
            if (string.IsNullOrEmpty(userToRegister.Email))
                return Problem("User email address is missing!", null, 404);

            var user = await _userManager.FindByEmailAsync(userToRegister.Email);
            if (user != null)
                return Problem("User already exists, please try with a different email address!", null, 500);

            user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = userToRegister.Email,
                UserName = userToRegister.Email,
            };

            var userCreateResult = await _userManager.CreateAsync(user, userToRegister.Password);
            if (userCreateResult.Succeeded)
            {
                var userToReturn = _mapper.Map<UserAuthDto>(user);
                return CreatedAtRoute("GetUserRoute", new { id = user.Id },
                        new { Status = StatusCode(201), User = userToReturn, token = GenerateJwt(user) });
            }

            return Problem(userCreateResult.Errors.First().Description, null, 500);
        }

        [HttpPost("SignIn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn([FromBody] UserToLoginDto userLoginResource)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == userLoginResource.Email);
            if (user is null)
                return NotFound("User not found");

            var userSigninResult = await _userManager.CheckPasswordAsync(user, userLoginResource.Password);
            if (userSigninResult)
            {
                return Ok(new
                {
                    token = GenerateJwt(user),
                    user = _mapper.Map<UserAuthDto>(user)
                });
            }

            return BadRequest("Email or password incorrect.");
        }

        private string GenerateJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            //var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            //claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _config["Tokens:Issuer"],
                audience: _config["Tokens:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
