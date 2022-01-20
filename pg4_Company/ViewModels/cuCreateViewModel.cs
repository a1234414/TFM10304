using System.ComponentModel.DataAnnotations;

namespace Project_TFM10304.Controllers
{
    public class cuCreateViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "密碼長度需介於 {2} ~ {1} 字元之間", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "密碼不一致")]
        public string ConfirmPassword { get; set; }

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