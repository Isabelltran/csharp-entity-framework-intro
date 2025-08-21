using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace exercise.webapi.Repository
{
    public class BookRepository: IBookRepository
    {
        DataContext _db;
        
        public BookRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<Book> GetBook(int id)
        {
            return await _db.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _db.Books.Include(b => b.Author).ToListAsync();

        }
        public async Task<Book> UpdateAsync(int id, Author author)
        {
            var target = await _db.Books.FindAsync(id);
            target.Author = author;
            await _db.SaveChangesAsync();
            return target;
        }

        public async Task<Book> DeleteAsync(int id)
        {
            var entity = await _db.Books.FindAsync(id);
            if (entity == null) return null;
            _db.Books.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Book> AddAsync(Book book)
        {
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<Author> GetAuthor(int id)
        {
            return await _db.Authors.FindAsync(id);
        }
    }
}
