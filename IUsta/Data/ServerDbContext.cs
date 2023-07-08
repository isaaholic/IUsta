using IUsta.Models;
using Microsoft.EntityFrameworkCore;

namespace IUsta.Data
{
    public class ServerDbContext:DbContext
    {
        public ServerDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<WorkingArea> WorkingAreas { get; set; }
    }
}
