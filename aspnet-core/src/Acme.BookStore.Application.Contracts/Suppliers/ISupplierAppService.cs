using Acme.BookStore.Authors;
using Acme.BookStore.Suppliers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Suppliers
{
    public interface ISupplierAppService : IApplicationService
    {
        Task<SupplierDto> GetAsync(Guid id);

        Task<PagedResultDto<SupplierDto>> GetListAsync(GetSupplierListDto input);

        Task<SupplierDto> CreateAsync(CreateSupplierDto input);

        Task UpdateAsync(Guid id, UpdateSupplierDto input);

        Task DeleteAsync(Guid id);
    }
}
