using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.BookStore.Publishers
{
    public class UpdatePublisherDto
    {
        [Required]
        [StringLength(20)]
        public string Name { get; private set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
    }
}
