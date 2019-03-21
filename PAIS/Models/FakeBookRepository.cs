using System.Collections.Generic;
using System.Linq;

namespace PAIS.Models
{
    public class FakeBookRepository : IBookRepository
    {
        public IQueryable<Book> Books => new List<Book>
        {
            new Book { Name = "ЕНЦИКЛОПЕДІЯ АФОРИЗМІВ, КРИЛАТИХ ФРАЗ, ЦИТАТ", Price = 280 },
            new Book { Name = "ПРОШУ КОРОТКО", Price = 130 },
            new Book { Name = "Теорія і методика журналістської творчості", Price = 70 }
        }.AsQueryable<Book>();
    }
}
