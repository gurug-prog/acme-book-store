using Volo.Abp;

namespace Acme.BookStore.Suppliers;

public class SupplierAlreadyExistsException : BusinessException
{
    public SupplierAlreadyExistsException(string name)
        : base(BookStoreDomainErrorCodes.SupplierAlreadyExists)
    {
        WithData("name", name);
    }
}
