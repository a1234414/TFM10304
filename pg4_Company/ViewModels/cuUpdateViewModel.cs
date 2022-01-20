using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_TFM10304.ViewModels
{
    public class cuUpdateViewModel
    {
        [Required]
        [StringLength(8, ErrorMessage = "請填入{2}碼統一編號", MinimumLength = 8)]
        [Display(Name = "TaxId")]
        public string TaxId { get; set; }

        [Required]
        [Display(Name = "CompanyName")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Legal name")]
        public string LegalName { get; set; }

        [Required]
        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Display(Name = "Nation")]
        public string Nation { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}
