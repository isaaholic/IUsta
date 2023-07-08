using IUsta.Models.Base;

namespace IUsta.Models
{
    public class WorkingArea:BaseEntity
    {
        public string WorkAreaName { get; set; }

        public Admin Admin { get; set; }
        public Guid AdminId { get; set; }
    }
}