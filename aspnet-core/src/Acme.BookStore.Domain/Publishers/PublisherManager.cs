using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Acme.BookStore.Publishers;

public class PublisherManager : DomainService
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherManager(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public async Task<Publisher> CreateAsync(
        string name,
        string address,
        string contactPhone)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existingPublisher = await _publisherRepository.FindByNameAsync(name);
        if (existingPublisher != null)
        {
            throw new PublisherAlreadyExistsException(name);
        }

        return new Publisher(
            GuidGenerator.Create(),
            name,
            address,
            contactPhone
        );
    }

    public async Task ChangeNameAsync(
        [NotNull] Publisher publisher,
        [NotNull] string newName)
    {
        Check.NotNull(publisher, nameof(publisher));
        Check.NotNullOrWhiteSpace(newName, nameof(newName));

        var existingPublisher = await _publisherRepository.FindByNameAsync(newName);
        if (existingPublisher != null && existingPublisher.Id != publisher.Id)
        {
            throw new PublisherAlreadyExistsException(newName);
        }

        publisher.ChangeName(newName);
    }
}
