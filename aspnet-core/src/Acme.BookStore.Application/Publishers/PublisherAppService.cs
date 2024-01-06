using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Publishers;

[Authorize(BookStorePermissions.Publishers.Default)]
public class PublisherAppService : BookStoreAppService, IPublisherAppService
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly PublisherManager _publisherManager;

    public PublisherAppService(
        IPublisherRepository publisherRepository,
        PublisherManager publisherManager)
    {
        _publisherRepository = publisherRepository;
        _publisherManager = publisherManager;
    }

    public async Task<PublisherDto> GetAsync(Guid id)
    {
        var publisher = await _publisherRepository.GetAsync(id);
        return ObjectMapper.Map<Publisher, PublisherDto>(publisher);
    }

    public async Task<PagedResultDto<PublisherDto>> GetListAsync(GetPublisherListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Publisher.Name);
        }

        var publishers = await _publisherRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _publisherRepository.CountAsync()
            : await _publisherRepository.CountAsync(
                publisher => publisher.Name.Contains(input.Filter));

        return new PagedResultDto<PublisherDto>(
            totalCount,
            ObjectMapper.Map<List<Publisher>, List<PublisherDto>>(publishers)
        );
    }

    [Authorize(BookStorePermissions.Publishers.Create)]
    public async Task<PublisherDto> CreateAsync(CreatePublisherDto input)
    {
        var publisher = await _publisherManager.CreateAsync(
            input.Name,
            input.Address,
            input.ContactPhone
        );

        await _publisherRepository.InsertAsync(publisher);

        return ObjectMapper.Map<Publisher, PublisherDto>(publisher);
    }

    [Authorize(BookStorePermissions.Publishers.Edit)]
    public async Task UpdateAsync(Guid id, UpdatePublisherDto input)
    {
        var publisher = await _publisherRepository.GetAsync(id);

        if (publisher.Name != input.Name)
        {
            await _publisherManager.ChangeNameAsync(publisher, input.Name);
        }

        publisher.Address = input.Address;
        publisher.ContactPhone = input.ContactPhone;

        await _publisherRepository.UpdateAsync(publisher);
    }

    [Authorize(BookStorePermissions.Publishers.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _publisherRepository.DeleteAsync(id);
    }
}
