using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models.DBObjects
{
    public partial class Member
    {
        public Member()
        {
            Fees = new HashSet<Fee>();
            Reviews = new HashSet<Review>();
            Transactions = new HashSet<Transaction>();
        }

        public string Idmember { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Fee> Fees { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
