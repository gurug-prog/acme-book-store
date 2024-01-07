using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Data;
using Acme.BookStore.Permissions;
using Acme.BookStore.Publishers;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using static System.Net.Mime.MediaTypeNames;

namespace Acme.BookStore.Books;

[Authorize(BookStorePermissions.Books.Default)]
public class BookAppService :
    CrudAppService<
        Book, //The Book entity
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto>, //Used to create/update a book
    IBookAppService //implement the IBookAppService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly IBlobContainer<BookFileContainer> _blobContainer;

    public BookAppService(
        IRepository<Book, Guid> repository,
        IAuthorRepository authorRepository,
        IPublisherRepository publisherRepository,
        IBlobContainer<BookFileContainer> blobContainer)
        : base(repository)
    {
        _authorRepository = authorRepository;
        this._publisherRepository = publisherRepository;
        this._blobContainer = blobContainer;
        GetPolicyName = BookStorePermissions.Books.Default;
        GetListPolicyName = BookStorePermissions.Books.Default;
        CreatePolicyName = BookStorePermissions.Books.Create;
        UpdatePolicyName = BookStorePermissions.Books.Edit;
        DeletePolicyName = BookStorePermissions.Books.Delete;
    }

    public override async Task<BookDto> CreateAsync(CreateUpdateBookDto input)
    {
        var created = await base.CreateAsync(input);
        byte[] image = Convert.FromBase64String(input.CoverImage);
        await _blobContainer.SaveAsync(created.Id.ToString(), image, true);
        return created;
    }

    public override async Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input)
    {
        var updated = await base.UpdateAsync(id, input);
        byte[] image = Convert.FromBase64String(input.CoverImage);
        await _blobContainer.SaveAsync(updated.Id.ToString(), image, true);
        return updated;
    }

    public override async Task<BookDto> GetAsync(Guid id)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                    join publisher in await _publisherRepository.GetQueryableAsync() on book.PublisherId equals publisher.Id
                    where book.Id == id
                    select new { book, author, publisher };

        //Execute the query and get the book with author
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Book), id);
        }

        var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
        bookDto.AuthorName = queryResult.author.Name;
        bookDto.PublisherName = queryResult.publisher.Name;

        try
        {
            var image = await _blobContainer.GetAllBytesAsync(id.ToString());
            if (image != null)
            {
                bookDto.CoverImage = Convert.ToBase64String(image);
            }
        }
        catch (Exception ex) { }

        return bookDto;
    }

    public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                    join publisher in await _publisherRepository.GetQueryableAsync() on book.PublisherId equals publisher.Id
                    select new { book, author, publisher };

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of BookDto objects
        var bookDtos = queryResult.Select(x =>
        {
            var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
            bookDto.AuthorName = x.author.Name;
            bookDto.PublisherName = x.publisher.Name;
            return bookDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<BookDto>(
            totalCount,
            bookDtos
        );
    }

    public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
    {
        var authors = await _authorRepository.GetListAsync();

        return new ListResultDto<AuthorLookupDto>(
            ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
        );
    }

    public async Task<ListResultDto<PublisherLookupDto>> GetPublisherLookupAsync()
    {
        var publishers = await _publisherRepository.GetListAsync();

        return new ListResultDto<PublisherLookupDto>(
            ObjectMapper.Map<List<Publisher>, List<PublisherLookupDto>>(publishers)
        );
    }

    private static string NormalizeSorting(string sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"book.{nameof(Book.Name)}";
        }

        if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "authorName",
                "author.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        if (sorting.Contains("publisherName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "publisherName",
                "publisher.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"book.{sorting}";
    }
}
