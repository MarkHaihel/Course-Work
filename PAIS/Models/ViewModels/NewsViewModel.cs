using System.Collections.Generic;
using PAIS.Models;

namespace PAIS.Models.ViewModels
{
    public class NewsListViewModel
    {
        public IEnumerable<News> NewsRepo { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string Search { get; set; }
    }
}
