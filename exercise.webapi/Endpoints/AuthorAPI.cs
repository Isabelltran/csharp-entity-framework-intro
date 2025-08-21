using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace exercise.webapi.Endpoints
{
    public static class AuthorAPI
    {
        public static void ConfigureAuthorsApi(this WebApplication app)
        {
            var authors = app.MapGroup("authors");

            authors.MapGet("/", GetAuthors);
            authors.MapGet("/{id}", GetAuthorById);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAuthors (IAuthorRepository repository)
        {
            List<Object> authors = new List<Object>();
            var results = await repository.GetAll();
            foreach (Author author in results)
            {
                List<Object> bookDTO = new List<Object>();
                foreach(Book book in author.Books)
                {
                    bookDTO.Add(new {title = book.Title });  
                    
                }
                authors.Add(new {Id = author.Id, Firstname = author.FirstName, Lastname = author.LastName, Email = author.Email, Books = bookDTO });
            }
            return TypedResults.Ok(authors);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public static async Task<IResult> GetAuthorById(IAuthorRepository repository, int id)
        {
            var author = await repository.GetAuthorById(id);
            if (author == null) return TypedResults.NotFound(new { Error = "No Author Found!" });

            List<Object> bookDTO = new List<Object>();
            foreach (Book book in author.Books)
            {
                bookDTO.Add(new { title = book.Title });

            }

            var authorDTO = new { Id = author.Id, Firstname = author.FirstName, Lastname = author.LastName, Email = author.Email, Books = bookDTO };
            return TypedResults.Ok(authorDTO);
        }
    }
}

