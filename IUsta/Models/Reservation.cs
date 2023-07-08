using IUsta.Models.Base;

namespace IUsta.Models
{
    public class Reservation:BaseEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string Description { get; set; }

        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
