using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PAIS.Models;

namespace PAIS.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IBookRepository repository;
        public NavigationMenuViewComponent(IBookRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedType = RouteData?.Values["type"];
            return View(repository.Books
            .Select(x => x.PublicationType)
            .Distinct()
            .OrderBy(x => x));
        }
    }
}