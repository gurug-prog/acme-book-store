﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Publishers
{
    public class PublisherDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
    }
}
