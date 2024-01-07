using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.BookStore.Suppliers
{
    public class UpdateSupplierDto
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
        public int MinimumOrderQuantity { get; set; }
    }
}
