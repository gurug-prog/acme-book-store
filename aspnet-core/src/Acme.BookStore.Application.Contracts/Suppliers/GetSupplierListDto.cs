using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Suppliers;

public class GetSupplierListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
