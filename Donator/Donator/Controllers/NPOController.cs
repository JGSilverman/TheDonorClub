using AutoMapper;
using Donator.Data.Repos;
using Donator.Dtos.NPO;
using Donator.Extensions;
using Donator.Helpers;
using Donator.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Donator.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class NPOController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INPORepo _npoRepo;
        private readonly IUserRepo _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly IOrgUserRepo _orgUserRepo;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NPOController(
            IMapper mapper,
            INPORepo npoRepo,
            IUserRepo userRepo,
            UserManager<User> userManager,
            IOrgUserRepo orgUserRepo, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _npoRepo = npoRepo;
            _userRepo = userRepo;
            _userManager = userManager;
            _orgUserRepo = orgUserRepo;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET NPO LIST
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNPOs([FromQuery] PagingParams paging)
        {
            var npos = await _npoRepo.GetNPOs(paging);
            var nposToReturn = _mapper.Map<IEnumerable<NPODetailDto>>(npos);

            Response.AddPagination(
                npos.CurrentPage,
                npos.PageSize,
                npos.TotalCount,
                npos.TotalPages);

            return Ok(new
            {
                Status = StatusCode(200),
                NPOs = nposToReturn
            });
        }

        // GET NPO BY ID
        /// <summary>
        /// Get NPO By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNPOById(int id)
        {
            var NPO = await _npoRepo.GetNPOById(id);
            if (NPO == null)
                return NotFound(new { Status = StatusCode(404), Message = "Could not find NPO." });

            var NPOToReturn = _mapper.Map<NPODetailDto>(NPO);
            return Ok(new
            {
                Status = StatusCode(200),
                NPO = NPOToReturn
            });
        }

        // CREATE A NEW NPO
        /// <summary>
        /// Create NPO
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("CreateNPO")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNPO([FromBody] NPOtoCreateDto createDto)
        {
            var user = await _userManager.FindByIdAsync(createDto.CreatedByUserId);
            if (user is null)
                return NotFound("User not found");

            try
            {
                #region Validate Tax Id

                if (string.IsNullOrEmpty(createDto.TaxId))
                    return BadRequest(new
                    {
                        Status = StatusCode(400),
                        Success = false,
                        Message = "Tax Id cannot be null!"
                    });

                string url = string.Format(_config.GetValue<string>(
                    "TaxSettings:WebsiteUrl"), createDto.TaxId);

                if (Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "selenium-drivers")))
                {
                    #region Using Selenium Approach

                    ChromeDriverService service =
                        ChromeDriverService.CreateDefaultService(Path.Combine(_webHostEnvironment.WebRootPath,
                            "selenium-drivers"));
                    service.HideCommandPromptWindow = true;

                    var options = new ChromeOptions();
                    options.AddArgument("--window-position=-32000,-32000");

                    using var driver = new ChromeDriver(service, options);
                    driver.Navigate().GoToUrl(url);
                    //driver.FindElement(By.Id("ein1")).Click();
                    var textBox = driver.FindElement(By.Id("ein1"));
                    textBox.SendKeys(createDto.TaxId);

                    // click submit
                    driver.FindElement(By.Id("s")).Click();

                    var successResultElement = driver.FindElement(
                        By.ClassName("top-of-search-header"));

                    if (successResultElement != null)
                    {
                        var newNPO = await _npoRepo.CreateNewNPO(_mapper.Map<NPO>(createDto));
                        if (newNPO == null)
                            return BadRequest(new { Status = StatusCode(400) });

                        var userAdded = await _orgUserRepo.AddUserToOrgUserList(newNPO.Id, createDto.CreatedByUserId, true);

                        // we want to make sure there's a default user on the org, so if adding the user fails, 
                        // destroy the whole org and make the user recreate it
                        if (!userAdded)
                        {
                            var isDeleted = await _npoRepo.DeleteNPO(newNPO.Id);
                            if (isDeleted)
                                return BadRequest(new
                                {
                                    Status = StatusCode(400),
                                    Success = false,
                                    Message =
                                        "Adding you as a user of the organization failed. Try creating the organization again."
                                });
                            else
                                return BadRequest(new
                                {
                                    Status = StatusCode(400),
                                    Success = false,
                                    Message = "Could not delete organization."
                                });
                        }

                        return CreatedAtRoute(new { id = newNPO.Id },
                            new
                            {
                                Status = StatusCode(201),
                                Success = true,
                                NPO = _mapper.Map<NPODetailDto>(newNPO)
                            });
                    }

                    #endregion
                }
                else
                {
                    return Ok(new
                    {
                        Status = StatusCode(400),
                        Success = false,
                        Message = "Not able to find chrome drivers in the directory configured!"
                    });
                }

                #endregion
            }
            catch (NoSuchElementException ex)
            {
                return Ok(new
                {
                    Status = StatusCode(404),
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = StatusCode(400),
                    Success = false,
                    Message = ex.Message
                });
            }

            return BadRequest(new
            {
                Status = StatusCode(400),
                Success = false,
                Message = "An error accored while adding NPO!"
            });
        }

        // Search Tax Id
        /// <summary>
        /// Search Tax Id
        /// </summary>
        /// <param name="taxid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search/{taxid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SearchTaxIdAsync(string taxid)
        {
            string url = string.Format(_config.GetValue<string>(
                "TaxSettings:WebsiteUrl"), taxid);

            if (Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "selenium-drivers")))
            {
                #region Using Selenium Approach

                ChromeDriverService service =
                    ChromeDriverService.CreateDefaultService(Path.Combine(_webHostEnvironment.WebRootPath,
                        "selenium-drivers"));
                service.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--window-position=-32000,-32000");

                using var driver = new ChromeDriver(service, options);
                driver.Navigate().GoToUrl(url);
                //driver.FindElement(By.Id("ein1")).Click();
                var textBox = driver.FindElement(By.Id("ein1"));
                textBox.SendKeys(taxid);

                // click submit
                driver.FindElement(By.Id("s")).Click();

                try
                {
                    var successResultElement = driver.FindElement(
                        By.ClassName("top-of-search-header"));

                    if (successResultElement != null)
                        return Ok(new
                        {
                            Status = StatusCode(200),
                            Success = true,
                            Message = "Tax Id is valid!"
                        });
                }
                catch (NoSuchElementException ex)
                {
                    return Ok(new
                    {
                        Status = StatusCode(404),
                        Success = false,
                        Message = "Tax Id is invalid!"
                    });
                }

                #endregion
            }

            return Ok(new
            {
                Status = StatusCode(404),
                Success = false,
                Message = "Not able to find chrome drivers!"
            });
        }
    }
}
