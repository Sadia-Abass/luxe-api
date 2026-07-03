using luxe.Server.Application.DTOs;
using luxe.Server.Application.DTOs.Category;
using luxe.Server.Application.Repositories;
using luxe.Server.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace luxe.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        // GET: api/GetCategoryById/{id}
        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> GetCategoryById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryResponse = await _categoryRepository.GetCategoryByIdAsync(id);
            if (categoryResponse == null)
            {
                return NotFound();
            }
            
            return Ok(categoryResponse);
        }

        // GET: api/GetAllCategories
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDTO>>>> GetAllCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoriesResponse = await _categoryRepository.GetAllCategoriesAsync();
            if (categoriesResponse == null)
            {
                return NotFound();
            }

            return Ok(categoriesResponse);
        }

        // POST: api/CreateCategory
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<ApiResponse<CreateCategoryDTO>>> CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createCategoryResponse = await _categoryRepository.CreateCategoryAsync(createCategoryDTO);

            if (createCategoryResponse == null)
            {
                return BadRequest("Failed to create category.");
            }

            return Ok(createCategoryResponse);
        }

        // PUT: api/UpdateCategory
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ApiResponse<UpdateCategoryDTO>>> UpdateCategory([FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateCategoryResponse = await _categoryRepository.UpdateCategoryAsync(updateCategoryDTO);
            if (updateCategoryResponse == null)
            {
                return NotFound();
            }

            return Ok(updateCategoryResponse);
        }

        // DELETE: api/DeleteCategory/{id}
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteCategoryResponse = await _categoryRepository.DeleteCategoryAsync(id);
            if (deleteCategoryResponse == null)
            {
                return NotFound();
            }

            return Ok(deleteCategoryResponse);
        }
    }
}
