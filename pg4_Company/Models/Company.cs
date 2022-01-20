using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_TFM10304.Models
{
    public class Company
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual Users User { get; set; }
        public int TaxId { get; set; }
        public string CompanyName { get; set; }
        public string LegalName { get; set; }
        public string Nation { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
