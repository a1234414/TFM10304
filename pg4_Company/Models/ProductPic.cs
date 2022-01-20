using System.ComponentModel.DataAnnotations.Schema;

namespace Project_TFM10304.Models
{
    public class ProductPic
    {

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PicPath { get; set; }
        public Product Product { get; set; }

    }
}