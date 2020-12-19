using AutoMapper;
using Donator.Data.Repos;
using Donator.Dtos.OrgUser;
using Donator.Dtos.User;
using Donator.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Donator.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/orgusers")]
    [ApiController]
    public class OrgUsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INPORepo _npoRepo;
        private readonly IUserRepo _userRepo;
        public readonly UserManager<User> _userManager;
        public readonly IOrgUserRepo _orgUserRepo;

        public OrgUsersController(
            IMapper mapper,
            INPORepo npoRepo,
            IUserRepo userRepo,
            UserManager<User> userManager,
            IOrgUserRepo orgUserRepo)
        {
            _mapper = mapper;
            _npoRepo = npoRepo;
            _userRepo = userRepo;
            _userManager = userManager;
            _orgUserRepo = orgUserRepo;
        }

        // GET USER LIST FOR NPO
        [HttpGet("{npoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersForNPO(int npoId)
        {
            OrgUser userFromTokenIsInOrg = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, GetUserIdFromToken());
            if (userFromTokenIsInOrg == null) return Unauthorized(new { Status = StatusCode(401) });
            if (!userFromTokenIsInOrg.IsActive && !userFromTokenIsInOrg.IsAdminOfOrg) return Unauthorized(new { Status = StatusCode(401) });

            var userList = await _orgUserRepo.GetUsersForOrgByOrgId(npoId);

            var usersToReturn = _mapper.Map<IEnumerable<OrgUserForListDto>>(userList);
            return Ok(new
            {
                Status = StatusCode(200),
                users = usersToReturn
            });
        }

        // GET LIST OF NPOS A USER IS ASSOCIATED WITH
        [HttpGet("{userId}/npo-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNPOListForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Status = StatusCode(400), Message = "UserId can't be null or empty" });
            List<ListOfNPOsForOrgUser> npoList = await _orgUserRepo.GetListOfActiveNPOsUserIsAffiliatedWith(userId);
            return Ok(new
            {
                Status = StatusCode(200),
                Organizations = npoList
            });
        }

        [HttpPost("{npoId}/{userId}/AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddUserToNPO(int npoId, string userId)
        {
            var user = await _userRepo.GetUserById(userId);

            if (user == null)
            {
                return BadRequest(new { Status = false, Message = "User not found" });
            }

            var org = await _npoRepo.GetNPOById(npoId);

            if (org == null)
            {
                return BadRequest(new { Status = false, Message = "NPO not found" });
            }

            OrgUser userFromTokenIsInOrg = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, GetUserIdFromToken());
            if (userFromTokenIsInOrg == null) return Unauthorized(new { Status = StatusCode(401) });
            if (!userFromTokenIsInOrg.IsActive && !userFromTokenIsInOrg.IsAdminOfOrg) return Unauthorized(new { Status = StatusCode(401) });

            try
            {
                var userAddedToOrg = await _orgUserRepo.AddUserToOrgUserList(npoId, userId, false);
                if (userAddedToOrg)
                {
                    return Ok(new { Status = StatusCode(201) });
                }

                return BadRequest(new { Status = StatusCode(400), Message = "Adding the user failed." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{npoId}/{userId}/RemoveUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoveUser(int npoId, string userId)
        {
            OrgUser userFromTokenIsInOrg = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, GetUserIdFromToken());
            if (userFromTokenIsInOrg == null) return Unauthorized(new { Status = StatusCode(401) });
            if (!userFromTokenIsInOrg.IsActive && !userFromTokenIsInOrg.IsAdminOfOrg) return Unauthorized(new { Status = StatusCode(401) });

            var user = await _userRepo.GetUserById(userId);

            if (user == null)
            {
                return BadRequest(new { Status = false, Message = "User not found" });
            }

            var org = await _npoRepo.GetNPOById(npoId);

            if (org == null)
            {
                return BadRequest(new { Status = false, Message = "NPO not found" });
            }

            try
            {
                var userIsRemoved = await _orgUserRepo.RemoveUserFromOrgUserListAsync(npoId, userId);
                if (userIsRemoved)
                {
                    return Ok(new { Status = StatusCode(204) });
                }

                return BadRequest(new { Status = StatusCode(400), Message = "Removing the user failed." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST api/npo/5/addAdmin/3
        // FOR adding an admin to an npo
        [HttpPost("{npoId}/{userId}/MakeAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MakeAdmin([FromRoute] int npoId, string userId)
        {
            string idFromToken = GetUserIdFromToken();

            // check if user who sent the token is an admin of the npo 
            // only admins can add another admin to the npo
            OrgUser userFromTokenIsInOrg = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, idFromToken);
            if (userFromTokenIsInOrg == null) return Unauthorized(new { Status = StatusCode(401) });
            if (!userFromTokenIsInOrg.IsActive && !userFromTokenIsInOrg.IsAdminOfOrg) return Unauthorized(new { Status = StatusCode(401) });

            OrgUser userToBeNewAdmin = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, userId);

            if (userToBeNewAdmin == null) return BadRequest(new 
            { 
                Status = StatusCode(400), 
                Message = "The user you're trying to make as an admin is not part of the organization." 
            });

            try
            {
                var userAdded = await _orgUserRepo.MakeUserAdminOfOrgAsync(npoId, userId);
                if (!userAdded) return BadRequest(new { Status = StatusCode(400) });
                return Ok(new
                {
                    Status = StatusCode(201)
                });
            }
            catch (Exception)
            {
                return BadRequest(new { Status = StatusCode(400) });
                throw;
            }
        }

        [HttpDelete("{npoId}/{userId}/RescindAdminRights")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RescindAdminRights([FromRoute] int npoId, string userId)
        {
            OrgUser userFromTokenIsInOrg = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, GetUserIdFromToken());
            if (userFromTokenIsInOrg == null) return Unauthorized(new { Status = StatusCode(401) });
            if (!userFromTokenIsInOrg.IsActive && !userFromTokenIsInOrg.IsAdminOfOrg) return Unauthorized(new { Status = StatusCode(401) });

            var user = await _userRepo.GetUserById(userId);

            if (user == null)
            {
                return BadRequest(new { Status = false, Message = "User not found" });
            }

            var org = await _npoRepo.GetNPOById(npoId);

            if (org == null)
            {
                return BadRequest(new { Status = false, Message = "NPO not found" });
            }

            var countOfAdmins = await _orgUserRepo.GetCountOfAdminsForOrg(npoId);

            if (countOfAdmins <= 1) return BadRequest(new { Status = StatusCode(400), Message = "Must have a minimum of one admin"});

            try
            {
                var userIsNoLongerAdmin = await _orgUserRepo.RescindAdminPrivledgesOfUserAsync(npoId, userId);
                if (userIsNoLongerAdmin)
                {
                    return Ok(new { Status = StatusCode(204) });
                }

                return BadRequest(new { Status = StatusCode(400), Message = "Removing admin rights to the user failed." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("{npoId}/{userId}/DeactivateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeactivateUser([FromRoute] int npoId, string userId)
        {
            OrgUser userFromTokenIsInOrg = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, GetUserIdFromToken());
            if (userFromTokenIsInOrg == null) return Unauthorized(new { Status = StatusCode(401) });
            if (!userFromTokenIsInOrg.IsActive && !userFromTokenIsInOrg.IsAdminOfOrg) return Unauthorized(new { Status = StatusCode(401) });

            OrgUser userToBeDeactivatedIsInOrgUserList = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, userId);

            if (userToBeDeactivatedIsInOrgUserList == null) return BadRequest(new
            {
                Status = StatusCode(400),
                Message = "The user you're trying to deactivate is not part of the organization."
            });

            try
            {
                var userIsDeactivated = await _orgUserRepo.DeactivateUserFromOrgUserListAsync(npoId, userId);
                if (userIsDeactivated)
                {
                    return Ok(new { Status = StatusCode(204) });
                }

                return BadRequest(new { Status = StatusCode(400), Message = "Deactivating the user failed." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("{npoId}/{userId}/ActivateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ActivateUser([FromRoute] int npoId, string userId)
        {
            OrgUser userFromTokenIsInOrg = await _orgUserRepo.GetUserInOrgByOrgIdAndUserId(npoId, GetUserIdFromToken());
            if (userFromTokenIsInOrg == null) return Unauthorized(new { Status = StatusCode(401) });
            if (!userFromTokenIsInOrg.IsActive && !userFromTokenIsInOrg.IsAdminOfOrg) return Unauthorized(new { Status = StatusCode(401) });

            var user = await _userRepo.GetUserById(userId);

            if (user == null)
            {
                return BadRequest(new { Status = false, Message = "User not found" });
            }

            var org = await _npoRepo.GetNPOById(npoId);

            if (org == null)
            {
                return BadRequest(new { Status = false, Message = "NPO not found" });
            }

            try
            {
                var userIsActivated = await _orgUserRepo.ActivateUserFromOrgUserListAsync(npoId, userId);
                if (userIsActivated)
                {
                    return Ok(new { Status = StatusCode(204) });
                }

                return BadRequest(new { Status = StatusCode(400), Message = "Activating the user failed." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetUserIdFromToken()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

    }
}
