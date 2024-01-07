using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Publishers;
using Acme.BookStore.Suppliers;
using AutoMapper;

namespace Acme.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();

        CreateMap<Author, AuthorDto>();
        CreateMap<Author, AuthorLookupDto>();

        CreateMap<Publisher, PublisherLookupDto>();
        CreateMap<Publisher, PublisherDto>();

        CreateMap<Supplier, SupplierDto>();
    }
}
