using Acme.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Publishers
{
    public class CreatePublisherDto
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
    }
}
