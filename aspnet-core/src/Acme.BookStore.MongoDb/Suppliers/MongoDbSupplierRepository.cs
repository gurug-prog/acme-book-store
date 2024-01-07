using Acme.BookStore.Books;
using Acme.BookStore.MongoDb;
using Acme.BookStore.Suppliers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.Suppliers;

internal class MongoDbSupplierRepository :
    MongoDbRepository<BookStoreMongoDbContext, Supplier, Guid>,
    ISupplierRepository
{
    public MongoDbSupplierRepository(
        IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Supplier> FindByNameAsync(string name)
    {
        var collection = await GetCollectionAsync();
        var cursor = await collection.FindAsync(Builders<Supplier>.Filter.Eq(x => x.Name, name));
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<List<Supplier>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter = null)
    {
        var collection = await GetCollectionAsync();
        return collection.AsQueryable()
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                publisher => publisher.Name.Contains(filter)
                )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToList();
    }
}
