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
    }
}
