using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Suppliers
{
    public class SupplierDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
        public int MinimumOrderQuantity { get; set; }
    }
}
