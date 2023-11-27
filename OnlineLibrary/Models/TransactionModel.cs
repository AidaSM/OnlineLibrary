using OnlineLibrary.Models.DBObjects;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class TransactionModel
    {
        public Guid Idtransaction { get; set; }
        public string Idmember { get; set; }
        public Guid Idbook { get; set; }
        [DisplayFormat(DataFormatString = "0:MM/dd/yyyy")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString = "0:MM/dd/yyyy")]
        [DataType(DataType.Date)]
        public DateTime Return { get; set; }
        public string? Status { get; set; }

        public virtual Book IdbookNavigation { get; set; } = null!;
        public virtual Member IdmemberNavigation { get; set; } = null!;
        public BookModel Book { get; internal set; }
    }
}
