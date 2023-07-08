using IUsta.Data;
using IUsta.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IUsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ServerDbContext _context;

        public ReservationsController(ServerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetAll()
            => await _context.Reservations.ToListAsync();

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetByCustomer(Guid customerId)
            => await _context.Reservations.Where(r=>r.CustomerId==customerId).ToListAsync();

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetByWorker(Guid workerId)
            => await _context.Reservations.Where(r=>r.WorkerId== workerId).ToListAsync();

        [HttpPost("add")]
        public async Task<ActionResult<List<Reservation>>> AddReservation(Reservation reservation)
        {
            var newReservation = new Reservation()
            {
                CreatedAt = DateTime.UtcNow,
                Customer = reservation.Customer,
                Worker = reservation.Worker,
                Description = reservation.Description
            };

            await _context.Reservations.AddAsync(newReservation);
            await _context.SaveChangesAsync();

            return Ok((await _context.Customers.FindAsync(newReservation.Customer.Id))?.Reservations);
        }

        [HttpPut("finish")]
        public async Task<ActionResult<List<Reservation>>> FinishReservation(Reservation reservation)
        {
            var mainReservation = await _context.Reservations.FindAsync(reservation.Id);

            if (mainReservation == null) return NotFound();

            mainReservation.FinishedAt = reservation.FinishedAt;

            _context.Update(mainReservation);
            await _context.SaveChangesAsync();

            return Ok((await _context.Customers.FindAsync(mainReservation.Customer.Id))?.Reservations);
        }
    }
}
