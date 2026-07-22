using CloudinaryDotNet.Actions;
using luxe.Server.Application.DTOs.Users;
using luxe.Server.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace luxe.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET /api/users?pageNumber=1&pageSize=10&search=sadia
        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.GetAllAsync();
            if (result.Count == 0) 
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET /api/users/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.GetUserByIdAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok( result);
        }

        [HttpPost("add-user")]
        [Authorize(Roles = "super Admin")]
        public async Task<IActionResult> AddUser([FromBody] AddUserDTO addUserDTO)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.AddNewUserAsync(addUserDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.UpdateUserAsync(id, updateUserDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // PUT /api/users/{id}/profile-image
        [HttpPut("{id}/profile-image")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> UpdateProfileImage([FromRoute] string id, [FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.UpdateProfileImageAsync(id, file);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // DELETE /api/users/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.DeleteUserAsync(id);
            if (!result.IsSuccess)
            {
               return BadRequest(result);
            }

            return Ok(result);
        }

        // POST /api/users/{id}/roles
        [HttpPost("{id}/roles")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AssignRole([FromRoute] string id, [FromBody] AssignRoleDTO assignRoleDTO) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.AssignRoleAsync(id, assignRoleDTO);
            if (!result.IsSuccess) 
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // DELETE /api/users/{id}/roles/{role}
        [HttpDelete("{id}/roles/{role}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> DeleteRole([FromRoute] string id, [FromBody] string role) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.RemoveRoleAsync(id, role);
            if (!result.IsSuccess) 
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new { message = "This is a protected endpoint. You are an admin!" });
        }
    }
}
