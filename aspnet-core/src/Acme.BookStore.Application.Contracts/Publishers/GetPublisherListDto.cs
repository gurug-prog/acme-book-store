using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Publishers;

public class GetPublisherListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
