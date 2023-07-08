using IUsta.Data;
using IUsta.Models;
using IUsta.Models.Dtos.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IUsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ServerDbContext _context;

        public AdminsController(ServerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<bool> AnyAdminExists()
            => await _context.Admins.AnyAsync();

        [HttpPost("register")]
        public async Task<ActionResult<Admin>> Register(AdminDto request)
        {
            if (await AnyAdminExists())
                return BadRequest("You can't register as Admin. Because already One admin exists.");

            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);

            if (string.IsNullOrWhiteSpace(request.Nickname) || request.Nickname.Length < 8)
                return BadRequest("Nickname must be equal or greater than 8");
            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
                return BadRequest("Password must be equal or greater than 8");

            Admin admin = new()
            {
                Nickname = request.Nickname,
                PasswordHash = passwordHash,
            };

            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();

            return Ok(admin);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Admin>> Login(AdminDto request)
        {
            var mainAdmin = await _context.Admins.FirstOrDefaultAsync(c => c.Nickname == request.Nickname);

            if (mainAdmin == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(request.Password, mainAdmin.PasswordHash))
                return BadRequest("Wrong Password");

            return Ok(mainAdmin);
        }

        [HttpDelete("remove")]
        public async Task<ActionResult<bool>> Remove(Guid id)
        {
            var mainAdmin = await _context.Admins.FindAsync(id);

            if (mainAdmin == null)
                return NotFound();

            _context.Admins.Remove(mainAdmin);
            var count = await _context.SaveChangesAsync();

            return count > 0;
        }
    }
}
