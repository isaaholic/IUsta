using IUsta.Models.Base;

namespace IUsta.Models;

public class Category:BaseEntity
{
    public string CategoryName { get; set; }

    public Admin Admin { get; set; }
    public Guid AdminId { get; set; }
}