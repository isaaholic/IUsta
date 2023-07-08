using IUsta.Data;
using IUsta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IUsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ServerDbContext _context;

        public CategoriesController(ServerDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
            => await _context.Categories.ToListAsync();

        [HttpPost("addcategory")]
        public async Task<ActionResult<List<Category>>> AddCategory(Guid adminId, string CategoryName)
        {
            var mainAdmin = await _context.Admins.FindAsync(adminId);

            if (mainAdmin == null)
                return NotFound();

            if (await _context.Categories.AnyAsync(c => c.CategoryName == CategoryName))
                return BadRequest("Already exists");

            Category category = new() { CategoryName = CategoryName };

            if (mainAdmin.Categories is null)
                mainAdmin.Categories = new List<Category>();
            mainAdmin.Categories.Add(category);

            _context.Update(category);
            await _context.SaveChangesAsync();

            return Ok(mainAdmin.Categories);
        }

        [HttpDelete("removeCategory")]
        public async Task<ActionResult<List<Category>>> RemoveCategory(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                return NotFound(categoryId);

            _context.Remove(category);

            return Ok(_context.Categories.ToListAsync());
        }
    }
}
