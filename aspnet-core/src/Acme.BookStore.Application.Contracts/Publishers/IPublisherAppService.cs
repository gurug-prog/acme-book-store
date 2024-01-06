using Acme.BookStore.Authors;
using Acme.BookStore.Publishers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Publishers
{
    public interface IPublisherAppService : IApplicationService
    {
        Task<PublisherDto> GetAsync(Guid id);

        Task<PagedResultDto<PublisherDto>> GetListAsync(GetPublisherListDto input);

        Task<PublisherDto> CreateAsync(CreatePublisherDto input);

        Task UpdateAsync(Guid id, UpdatePublisherDto input);

        Task DeleteAsync(Guid id);
    }
}
