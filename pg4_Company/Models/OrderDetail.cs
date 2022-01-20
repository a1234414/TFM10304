using System.ComponentModel.DataAnnotations.Schema;

namespace Project_TFM10304.Models
{
    public class OrderDetail
    {
        [ForeignKey("Order")]
        //有改model 從int > string
        public string OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}