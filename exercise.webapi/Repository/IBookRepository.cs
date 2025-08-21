using exercise.webapi.Models;

namespace exercise.webapi.Repository
{
    public interface IBookRepository
    {
        Task<Book> GetBook(int id);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> UpdateAsync(int id, Author author);

        Task<Book> DeleteAsync(int id);

        Task<Book> AddAsync(Book book);

        Task<Author> GetAuthor(int id);
    }
}
