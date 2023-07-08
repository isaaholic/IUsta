using IUsta.Data;
using IUsta.Models.Dtos.Admin;
using IUsta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IUsta.Models.Dtos.Customer;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IUsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ServerDbContext _context;

        public CustomersController(ServerDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Customer>> Register(CustomerDto request)
        {
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);

            if (string.IsNullOrWhiteSpace(request.Email) || request.Email.Length < 8)
                return BadRequest("Nickname must be equal or greater than 8");
            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
                return BadRequest("Password must be equal or greater than 8");

            Customer customer = new()
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                Name = request.Name,
                Lastname = request.Lastname,
                ImagePath = request.ImagePath
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(CustomerDto request)
        {
            var mainCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);

            if (mainCustomer == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(request.Password, mainCustomer.PasswordHash))
                return BadRequest("Wrong Password");

            string token = CreateToken(mainCustomer);

            return Ok(token);
        }

        [HttpDelete("remove")]
        public async Task<ActionResult<bool>> Remove(Guid id)
        {
            var mainCustomer = await _context.Customers.FindAsync(id);

            if (mainCustomer == null)
                return NotFound();

            _context.Customers.Remove(mainCustomer);
            var count = await _context.SaveChangesAsync();

            return count > 0;
        }

        private string CreateToken(Customer customer)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role,"Customer")
            };


            byte[] keyBytes;
            using (var provider = new RNGCryptoServiceProvider())
            {
                int keySizeInBytes = 64;
                keyBytes = new byte[keySizeInBytes];
                provider.GetBytes(keyBytes);
            }

            var key = new SymmetricSecurityKey(keyBytes);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(4),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
