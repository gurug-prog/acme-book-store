using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Acme.BookStore.Suppliers;

public class SupplierManager : DomainService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierManager(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<Supplier> CreateAsync(
        string name,
        string address,
        string contactInformation,
        int moq)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingSupplier = await _supplierRepository.FindByNameAsync(name);
        if (existingSupplier != null)
        {
            throw new SupplierAlreadyExistsException(name);
        }

        return new Supplier(
            GuidGenerator.Create(),
            name,
            address,
            contactInformation,
            moq
        );
    }

    public async Task ChangeNameAsync(
        [NotNull] Supplier supplier,
        [NotNull] string newName)
    {
        Check.NotNull(supplier, nameof(supplier));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingSupplier = await _supplierRepository.FindByNameAsync(newName);
        if (existingSupplier != null && existingSupplier.Id != supplier.Id)
        {
            throw new SupplierAlreadyExistsException(newName);
        }

        supplier.ChangeName(newName);
    }
}
