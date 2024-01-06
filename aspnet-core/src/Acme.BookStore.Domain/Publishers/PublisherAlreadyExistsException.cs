using Volo.Abp;

namespace Acme.BookStore.Publishers;

public class PublisherAlreadyExistsException : BusinessException
{
    public PublisherAlreadyExistsException(string name)
        : base(BookStoreDomainErrorCodes.PublisherAlreadyExists)
    {
        WithData("name", name);
    }
}
