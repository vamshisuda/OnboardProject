using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserAPI.Interfaces;
using UserAPI.DTOs;
using UserAPI.Services;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;

        private readonly UserServices _userServices;

        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository, UserServices userServices)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int PageSize, int PageNumber)
        {
            try
            {

                var outputModel = await _userServices.GetpagedData( PageSize, PageNumber);

                Response.Headers.Add("Pagination", outputModel.Paging.ToJson());

               
                return Ok(new {
                    Success = true,
                    Message = "all users returned.",
                    outputModel
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            try
            {
                var Data = await _userRepository.GetById(userId);
                if(Data == null)
                    return NotFound(new
                    {
                    Success = false,
                    Message = "user not found with Id = " + userId
                    });
                else
                    return Ok(new {
                    Success = true,
                    Message = "User fetched.",
                    Data
                 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO createUserDTO)
        {
            try
            {
                var userId = await _userRepository.Create(createUserDTO);
                if (userId > 0)
                    return Ok(new
                    {
                        Success = true,
                        Message = "User created.",
                        userId
                    });
                else 
                    return Ok(new { Success=false,Message = "error while creating User" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        [Route("{userId}")]
        public async Task<IActionResult> Update(GeneralUserDTO updateUserDTO, int userId)
        {
            try
            {
               var updatedUser =  await _userRepository.Update(updateUserDTO, userId);
                if(updatedUser == null)
                    return NotFound(new
                    {
                        Success = false,
                        Message = "User  not found."
                        
                    });
                else
                return Ok(new
                {
                    Success = true,
                    Message = "User  updated.",
                    updatedUser
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{userid}")]
        public async Task<IActionResult> Delete(int userid)
        {
            try
            {
                var deletedUser = await _userRepository.Delete(userid);
                if(deletedUser == null)
                    return NotFound(new
                    {
                        Success = false,
                        Message = "User doesn't exist with id = "+userid+"."
                    });
                else
                return Ok(new
                {
                    Success = true,
                    Message = "User deleted.",
                    deletedUser
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(GeneralUserDTO generalDTO)
        {
            try
            {
                var Data = await _userRepository.Search(generalDTO);
                return Ok(new
                {
                    Success = true,
                    Message = "all users returned.",
                    Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

       
    }
}
