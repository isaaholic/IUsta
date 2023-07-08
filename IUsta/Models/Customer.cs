using IUsta.Models.Base;

namespace IUsta.Models;

public class Customer:BaseEntity
{
    public string ImagePath { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public WorkingArea? WorkingArea { get; set; }
    public string? CompanyName { get; set; }
    public List<Worker>? FavoriteWorkers { get; set; }
    public List<Reservation>? Reservations { get; set; }
}
