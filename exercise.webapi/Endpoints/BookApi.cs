using exercise.webapi.DTOs;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace exercise.webapi.Endpoints
{
    public static class BookApi
    {
        public static void ConfigureBooksApi(this WebApplication app)
        {
            var books = app.MapGroup("books");

            books.MapGet("/", GetBooks);
            books.MapGet("/{id}", GetBookById);
            books.MapPut("/{id}", Update);
            books.MapDelete("/{id}", Delete);
            books.MapPost("/", AddBook);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetBookById(IBookRepository bookRepository, int id)
        {
            var item = await bookRepository.GetBook(id);
            if (item == null) return TypedResults.NotFound(new { Error = "No Book Found!" });
            return TypedResults.Ok(item);
        }

        private static async Task<IResult> GetBooks(IBookRepository bookRepository)
        {
            var books = await bookRepository.GetAllBooks();
            return TypedResults.Ok(books);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public static async Task<IResult> Update(IBookRepository repository, int id, AuthorPut model)
        {
            var entity = await repository.GetBook(id);
            if (entity == null) return TypedResults.NotFound(new { Error = "No Books Found!" });

            if (model.FirstName != null) entity.Author.FirstName = model.FirstName;
            if (model.LastName != null) entity.Author.LastName = model.LastName;
            if (model.Email != null) entity.Author.Email = model.Email;

            await repository.UpdateAsync(id, entity.Author);
            return TypedResults.Created($"https://localhost:7188/products/{entity.Id}", new { Firstname = model.FirstName, LastName = model.LastName, Email = model.Email });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IBookRepository repository, int id)
        {
            var entity = await repository.DeleteAsync(id);
            if (entity == null) return TypedResults.NotFound(new { Error = "Book not found." });
            return TypedResults.Ok(entity);
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> AddBook(IBookRepository repository, BookPost model)
        {
            //  it should return NotFound when author id is not valid and
            var item = await repository.GetAuthor(model.AuthorId);
            if (item == null) return TypedResults.NotFound(new { Error = "Author not found" });

            //  BadRequest when book object not valid
            if (model.Title == "") return TypedResults.BadRequest(new { Error = "Need to type title" });

            Book entity = new Book();
            entity.Id = model.Id;
            entity.Title = model.Title;
            entity.AuthorId = model.AuthorId;

            var results = await repository.AddAsync(entity);

            return TypedResults.Created($"https://localhost:7188/products/{entity.Id}", new { Id = model.Id, Title = model.Title, Author = model.AuthorId });
        

        }
    }
}
