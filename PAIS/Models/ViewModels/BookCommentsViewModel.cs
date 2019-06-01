using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAIS.Models.ViewModels
{
    public class BookCommentsViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
