using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace luxe.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "This is a public endpoint. No authentication required." });
        }

        [Authorize]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            return Ok(new { message = "This is a protected endpoint. You are authenticated!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new { message = "This is a protected endpoint. You are an admin!" });
        }
    }
}
