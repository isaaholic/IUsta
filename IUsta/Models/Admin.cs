using IUsta.Models.Base;
namespace IUsta.Models;

public class Admin : BaseEntity
{
    public string Nickname { get; set; }
    public string PasswordHash { get; set; }
    public List<Category>? Categories { get; set; }
    public List<WorkingArea>? WorkingAreas { get; set; }
}
