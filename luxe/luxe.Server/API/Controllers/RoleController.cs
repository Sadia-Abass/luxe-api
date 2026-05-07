using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.AuthenticationDTOs;
using luxe.Server.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace luxe.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO createRoleDTO)
        {
            var result = await _roleRepository.CreateRoleAsync(createRoleDTO.RoleName);
            if (!result.IsSuccess)
            {
                return StatusCode((int)result.StatusCode, new { success = false, errors = result.ErrorMessages });
            }

            return Ok(new { success = true, message = result.ErrorMessages.FirstOrDefault(), data = result.Data });
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO assignRoleDTO)
        {
            var result = await _roleRepository.AssignRoleAsync(assignRoleDTO);
            if (!result.IsSuccess)
            {
                return StatusCode((int)result.StatusCode, new { success = false, errors = result.ErrorMessages });
            }

            return Ok(new { success = true, message = result.ErrorMessages.FirstOrDefault(), data = result.Data });
        }
    }
}
