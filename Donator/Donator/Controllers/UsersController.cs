using AutoMapper;
using Donator.Data.Repos;
using Donator.Dtos.User;
using Donator.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;

        public UsersController(IMapper mapper, IUserRepo userRepo)
        {
            _mapper = mapper;
            _userRepo = userRepo;
        }

        // GET USER BY ID
        [HttpGet("{id}", Name = "GetUserRoute")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) return BadRequest(new { Status = StatusCode(400), Message = "User Id can't be null" });
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                return NotFound(new { Status = StatusCode(404), Message = "Could not find user." });
            }

            var userToReturn = _mapper.Map<UserDetailDto>(user);

            return Ok(new
            {
                Status = StatusCode(200),
                User = userToReturn
            });
        }


    }
}
