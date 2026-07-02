using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Subcategory;
using luxe.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace luxe.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly SubcategoryRepostory _subcategoryRepostory;

        public SubcategoryController(SubcategoryRepostory subcategoryRepostory)
        {
            this._subcategoryRepostory = subcategoryRepostory ?? throw new ArgumentNullException(nameof(subcategoryRepostory));
        }

        // GET: api/GetSubcategoryById/{id}
        [HttpGet("GetSubcategoryById/{id}")]
        public async Task<ActionResult<ApiResponse<SubcategoryDTO>>> GetSubcategoryById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subcategoryResponse = await _subcategoryRepostory.GetSubcategoryByIdAsync(id);
            if (subcategoryResponse == null)
            {
                return NotFound();
            }

            return Ok(subcategoryResponse);
        }

        // GET: api/GetAllSubcategories
        [HttpGet("GetAllSubcategories")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SubcategoryDTO>>>> GetAllSubcategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subcategoriesResponse = await _subcategoryRepostory.GetAllSubcategoriesAsync();
            if (subcategoriesResponse == null)
            {
                return NotFound();
            }

            return Ok(subcategoriesResponse);
        }

        // POST: api/CreateSubcategory
        [HttpPost("CreateSubcategory")]
        public async Task<ActionResult<ApiResponse<CreateSubcategoryDTO>>> CreateSubcategory([FromBody] CreateSubcategoryDTO createSubcategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createSubcategoryResponse = await _subcategoryRepostory.CreateSubcategoryAsync(createSubcategoryDTO);

            if (createSubcategoryResponse == null)
            {
                return BadRequest("Failed to create subcategory.");
            }

            return Ok(createSubcategoryResponse);
        }

        // PUT: api/UpdateSubcategory
        [HttpPut("UpdateSubcategory")]
        public async Task<ActionResult<ApiResponse<UpdateSubcategoryDTO>>> UpdateSubcategory([FromBody] UpdateSubcategoryDTO updateSubcategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateSubcategoryResponse = await _subcategoryRepostory.UpdateSubcategoryAsync(updateSubcategoryDTO);

            if (updateSubcategoryResponse == null)
            {
                return NotFound();
            }

            return Ok(updateSubcategoryResponse);
        }

        // DELETE: api/DeleteSubcategory
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSubcategory(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteSubcategoryResponse = await _subcategoryRepostory.DeleteSubcategoryAsync(id);
            if (deleteSubcategoryResponse == null)
            {
                return NotFound();
            }

            return Ok(deleteSubcategoryResponse);
        }
    }
}
