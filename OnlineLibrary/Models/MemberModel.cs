using OnlineLibrary.Models.DBObjects;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class MemberModel
    {
        public string Idmember { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        [DisplayFormat(DataFormatString = "0:MM/dd/yyyy")]
        [DataType(DataType.Date)]
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

    }
}
