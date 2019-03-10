using System.Linq;

namespace PAIS.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
    }
}
