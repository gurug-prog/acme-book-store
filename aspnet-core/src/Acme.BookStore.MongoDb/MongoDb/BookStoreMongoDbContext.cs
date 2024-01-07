using Acme.BookStore.Suppliers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.MongoDb
{
    [ConnectionStringName("MongoDb")]
    internal class BookStoreMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<Supplier> Suppliers => Collection<Supplier>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            //Customize the configuration for your collections.
        }
    }
}
