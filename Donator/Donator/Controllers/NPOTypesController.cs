using AutoMapper;
using Donator.Data.Repos;
using Donator.Dtos.NPOType;
using Donator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Controllers
{
    [Route("api/npotypes")]
    [ApiController]
    public class NPOTypesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INPOTypeRepo _npoRepo;
        private readonly IUserRepo _userRepo;
        public readonly UserManager<User> _userManager;

        public NPOTypesController(IMapper mapper, INPOTypeRepo npoRepo, IUserRepo userRepo, UserManager<User> userManager)
        {
            _mapper = mapper;
            _npoRepo = npoRepo;
            _userRepo = userRepo;
            _userManager = userManager;
        }

        // GET Type BY ID
        [HttpGet("{id}", Name = "GetNPOType")]
        [ProducesResponseType(typeof(NPO), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNPOTypeById(int id)
        {
            var NPO = await _npoRepo.GetNPOTypeById(id);
            if (NPO == null)
                return NotFound(new { Status = StatusCode(404), Message = "Could not find NPO type." });

            return Ok(new
            {
                Status = StatusCode(200),
                NPO = NPO
            });
        }

        // FOR retrieving all types of npos
        [HttpGet(Name = "GetAllNPOTypes")]
        [ProducesResponseType(typeof(List<NPOType>), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllNPOTypes()
        {
            List<NPOType> types;

            types = await _npoRepo.GetAllNPOTypes();
            return Ok(new
            {
                Status = StatusCode(200),
                NPOTypes = types
            });
        }

        // CREATE A NEW NPO TYPE
        [Route("CreateNPOType")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNPOType([FromBody] NPOTypeToCreateDto createDto)
        {
            try
            {
                var newNPO = await _npoRepo.CreateNPOType(_mapper.Map<NPOType>(createDto));
                if (newNPO == null)
                    return BadRequest(new { Status = StatusCode(400) });

                return CreatedAtRoute("GetNPOType", new { id = newNPO.Id },
                    new { Status = StatusCode(201), NPO = newNPO });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = StatusCode(400), Message = ex.Message });
            }
        }

        // Update an NPO Type
        [HttpPost("{npoTypeId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNPOType(int npoTypeId, NPOTypeToUpdateDto updateDto)
        {
            var typeFromRepo = await _npoRepo.GetNPOTypeById(npoTypeId);
            if (typeFromRepo == null)
                return NotFound(new { Status = StatusCode(404), Message = "Could not find NPO type." });

            _mapper.Map(updateDto, typeFromRepo);

            var status = await _npoRepo.UpdateNPOType(typeFromRepo);
            if (!status)
            {
                return BadRequest(new { Status = false });
            }

            return Ok(new
            {
                Status = StatusCode(204),
                UpdatedNPOType = typeFromRepo
            });
        }

        // Delete an NPO Type
        [HttpDelete("{npoTypeId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNPOType(int npoTypeId)
        {
            var type = await _npoRepo.GetNPOTypeById(npoTypeId);
            if (type == null)
                return NotFound(new { Status = StatusCode(404), Message = "Could not find NPO type." });

            var status = await _npoRepo.DeleteNPOType(npoTypeId);

            if (!status)
            {
                return BadRequest(new { Status = false });
            }

            return Ok(new
            {
                Status = StatusCode(204)
            });
        }
    }
}
