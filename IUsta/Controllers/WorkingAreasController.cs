using IUsta.Data;
using IUsta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IUsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingAreasController : ControllerBase
    {

        private readonly ServerDbContext _context;

        public WorkingAreasController(ServerDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<WorkingArea>>> GetAll()
            => await _context.WorkingAreas.ToListAsync();

        [HttpPost("addWorkarea")]
        public async Task<ActionResult<List<WorkingArea>>> AddWorkAreas(Guid adminId, string WAName)
        {
            var mainAdmin = await _context.Admins.FindAsync(adminId);

            if (mainAdmin == null)
                return NotFound();

            if (await _context.WorkingAreas.AnyAsync(c => c.WorkAreaName == WAName))
                return BadRequest("Already exists");

            WorkingArea workingArea = new() { WorkAreaName = WAName };

            if (mainAdmin.WorkingAreas is null)
                mainAdmin.WorkingAreas = new List<WorkingArea>();
            mainAdmin.WorkingAreas.Add(workingArea);

            _context.Update(workingArea);
            await _context.SaveChangesAsync();

            return Ok(mainAdmin.WorkingAreas);
        }

        [HttpDelete("removeWorking")]
        public async Task<ActionResult<List<WorkingArea>>> RemoveWorkingArea(Guid waId)
        {
            var wa = await _context.WorkingAreas.FindAsync(waId);
            if (wa == null)
                return NotFound(waId);
            _context.Remove(wa);

            return Ok(_context.WorkingAreas.ToListAsync());
        }
    }
}
