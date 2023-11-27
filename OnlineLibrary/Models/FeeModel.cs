using OnlineLibrary.Models.DBObjects;

namespace OnlineLibrary.Models
{
    public class FeeModel
    {
        public Guid Idfee { get; set; }
        public Guid Idbook { get; set; }
        public string Idmember { get; set; }
        public string? Description { get; set; }
        public int IsPaid { get; set; }
        public decimal Amount { get; set; }

        public virtual Book IdbookNavigation { get; set; } = null!;
        public virtual Member IdmemberNavigation { get; set; } = null!;

    }
}
