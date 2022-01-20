using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pg4_Company.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required]
        public List<int> ProductIds { get; set; }
        [Required]
        public List<int> Qtys { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public List<string> ProductNames { get; set; }
        [Required]
        public List<int> ProductPrices { get; set; }

        //收件人姓名
        [Required]
        public string fReceiver { get; set; }

        //收件人電話
        [Required]
        public string fPhone { get; set; }

        //收件人Email
        [Required]
        public string fEmail { get; set; }

        //收件人地址
        [Required]
        public string fAddress { get; set; }
    }
}
