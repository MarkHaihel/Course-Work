using System.Linq;

namespace PAIS.Models
{
    interface IBookRepository
    {
        IQueryable<Book> Books { get; }
    }
}
