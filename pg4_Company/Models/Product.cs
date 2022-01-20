using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_TFM10304.Models
{
    public class Product
    {
        //Product ID
        [Key]                                           
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //Company
        [ForeignKey("Company")]
        public string CompanyUserId { get; set; }
        public virtual Company Company { get; set; }
        //代售庫存
        public int StockForSale { get; set; }  
        //總庫存
        public int TotalStock { get; set; }
        //上架狀態
        public bool IsSold { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        //簡述
        public string Description_S { get; set; }
        //詳述
        public string Description_L { get; set; }
        public string Description_L_1 { get; set; }
        public string Description_L_2 { get; set; }
        public string Description_L_3 { get; set; }
        public string Description_L_4 { get; set; }
        public string Description_L_5 { get; set; }
        //種類
        public string Type { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ProductPic> ProductPic { get; set; }
    }
}
