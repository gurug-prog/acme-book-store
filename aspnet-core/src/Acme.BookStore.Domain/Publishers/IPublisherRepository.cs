using Acme.BookStore.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Publishers
{
    public interface IPublisherRepository : IRepository<Publisher, Guid>
    {
        Task<Publisher> FindByNameAsync(string name);

        Task<List<Publisher>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter = null
        );
    }
}
