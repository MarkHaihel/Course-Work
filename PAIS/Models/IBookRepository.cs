using System.Linq;

namespace PAIS.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }

        void SaveBook(Book book);

        Book DeleteBook(int bookID);

        Book GetBook(int bookID);
    }
}
