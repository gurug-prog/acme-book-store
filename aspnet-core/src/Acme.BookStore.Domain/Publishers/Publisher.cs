using Acme.BookStore.Authors;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Publishers
{
    public class Publisher : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }

        private Publisher()
        {
            /* This constructor is for deserialization / ORM purpose */
        }

        internal Publisher(
            Guid id,
            [NotNull] string name,
            [CanBeNull] string? address = null,
            [CanBeNull] string? contactPhone = null)
            : base(id)
        {
            Name = name;
            Address = address;
            ContactPhone = contactPhone;
        }

        internal Publisher ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }

        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AuthorConsts.MaxNameLength
            );
        }
    }
}
