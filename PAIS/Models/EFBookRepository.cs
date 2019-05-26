using System.Collections.Generic;
using System.Linq;

namespace PAIS.Models
{
    public class EFBookRepository: IBookRepository
    {
        private ApplicationDbContext context;

        public EFBookRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Book> Books => context.Books;

        public void SaveBook(Book book)
        {
            if (book.BookID == 0)
            {
                context.Books.Add(book);
            }
            else
            {
                Book dbEntry = context.Books
                    .FirstOrDefault(p => p.BookID == book.BookID);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Description = book.Description;
                    dbEntry.Author = book.Author;
                    dbEntry.Format = book.Format;
                    dbEntry.PublicationDate = book.PublicationDate;
                    dbEntry.PublicationType = book.PublicationType;
                    dbEntry.Binder = book.Binder;
                    dbEntry.Amount = book.Amount;
                    dbEntry.Anotation = book.Anotation;
                    dbEntry.ISBNCode = book.ISBNCode;
                    dbEntry.Price = book.Price;
                }
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int bookID)
        {
            Book dbEntry = context.Books
                .FirstOrDefault(p => p.BookID == bookID);

            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            
            return dbEntry;
        }

        public Book GetBook(int bookID)
        {
            return Books
                .Where(n => n.BookID == bookID)
                .OrderBy(n => n.BookID)
                .First();
        }
    }
}
