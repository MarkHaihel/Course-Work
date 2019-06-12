using System.Collections.Generic;
using PAIS.Models;

namespace PAIS.Models.ViewModels
{
    public class BooksListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public BooksSortViewModel BooksSortVM { get; set; }
        public string Search { get; set; }
        public string Type { get; set; }
    }
}