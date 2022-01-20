using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_TFM10304.Models
{
    public class Order
    {
        [Key]
        //有修改model
        public string OrderId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public Users User { get; set; }
        public DateTime Date { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
        public int Amount { get; set; }
        public string fReceiver { get; set; }
        public string fPhone { get; set; }
        public string fEmail { get; set; }
        public string fAddress { get; set; }
        public bool IsPaid { get; set; }
    }
}
