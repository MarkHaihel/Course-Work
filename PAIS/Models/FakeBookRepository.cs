using System.Collections.Generic;
using System.Linq;

namespace PAIS.Models
{
    public class FakeBookRepository : IBookRepository
    {
        public IQueryable<Book> Books => new List<Book>
        {
            new Book { Name = "Football", Price = 25 },
            new Book { Name = "Surf board", Price = 179 },
            new Book { Name = "Running shoes", Price = 95 }
        }.AsQueryable<Book>();
    }
}
