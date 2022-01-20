using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pg4_Company.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { set; get; }
        [Required]
        public int Price { set; get; }
        public bool IsSold { set; get; }

        public string Type { set; get; }

        public int  TotalStock { set; get; }
        public string Location { set; get; }
        [Required]
        public string Description_S { set; get; }
        public string Description_L { set; get; }
        public string Description_L_1 { set; get; }
        public string Description_L_2 { set; get; }
        public string Description_L_3 { set; get; }
        public string Description_L_4 { set; get; }
        public string Description_L_5 { set; get; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public List<IFormFile> Pic { set; get; }
    }
}
