using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.MongoDb
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class BookStoreMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });
        }
    }
}
