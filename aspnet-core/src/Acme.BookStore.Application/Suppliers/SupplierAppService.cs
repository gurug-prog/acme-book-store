using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Suppliers;

[Authorize(BookStorePermissions.Suppliers.Default)]
public class SupplierAppService : BookStoreAppService, ISupplierAppService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly SupplierManager _supplierManager;

    public SupplierAppService(
        ISupplierRepository supplierRepository,
        SupplierManager supplierManager)
    {
        _supplierRepository = supplierRepository;
        _supplierManager = supplierManager;
    }

    public async Task<SupplierDto> GetAsync(Guid id)
    {
        var supplier = await _supplierRepository.GetAsync(id);
        return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
    }

    public async Task<PagedResultDto<SupplierDto>> GetListAsync(GetSupplierListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Supplier.Name);
        }

        var suppliers = await _supplierRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _supplierRepository.CountAsync()
            : await _supplierRepository.CountAsync(
                supplier => supplier.Name.Contains(input.Filter));

        return new PagedResultDto<SupplierDto>(
            totalCount,
            ObjectMapper.Map<List<Supplier>, List<SupplierDto>>(suppliers)
        );
    }

    [Authorize(BookStorePermissions.Suppliers.Create)]
    public async Task<SupplierDto> CreateAsync(CreateSupplierDto input)
    {
        var supplier = await _supplierManager.CreateAsync(
            input.Name,
            input.Address,
            input.ContactInformation,
            input.MinimumOrderQuantity
        );

        await _supplierRepository.InsertAsync(supplier);

        return ObjectMapper.Map<Supplier, SupplierDto>(supplier);
    }

    [Authorize(BookStorePermissions.Suppliers.Edit)]
    public async Task UpdateAsync(Guid id, UpdateSupplierDto input)
    {
        var supplier = await _supplierRepository.GetAsync(id);

        if (supplier.Name != input.Name)
        {
            await _supplierManager.ChangeNameAsync(supplier, input.Name);
        }

        supplier.Address = input.Address;
        supplier.ContactInformation = input.ContactInformation;
        supplier.MinimumOrderQuantity = input.MinimumOrderQuantity;

        await _supplierRepository.UpdateAsync(supplier);
    }

    [Authorize(BookStorePermissions.Suppliers.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _supplierRepository.DeleteAsync(id);
    }
}
