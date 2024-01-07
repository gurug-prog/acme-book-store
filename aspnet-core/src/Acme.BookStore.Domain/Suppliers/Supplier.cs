using Acme.BookStore.Authors;
using Acme.BookStore.Suppliers;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;

namespace Acme.BookStore.Suppliers
{
    public class Supplier : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
        public int MinimumOrderQuantity { get; set; }

        private Supplier()
        {
            /* This constructor is for deserialization / ORM purpose */
        }

        internal Supplier(
            Guid id,
            [NotNull] string name,
            [CanBeNull] string? address = null,
            [CanBeNull] string? contactInformation = null,
            int moq = 0)
            : base(id)
        {
            Name = name;
            Address = address;
            ContactInformation = contactInformation;
            MinimumOrderQuantity = moq;
        }

        internal Supplier ChangeName([NotNull] string name)
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
