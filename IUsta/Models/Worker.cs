using IUsta.Models.Base;

namespace IUsta.Models;

public class Worker:BaseEntity
{
    public string? ImagePath { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public WorkingArea WorkingArea { get; set; }
    public Category Category { get; set; }
    public List<Reservation>? Reservations { get; set; }
    public List<Customer>? Followers { get; set; }
}
