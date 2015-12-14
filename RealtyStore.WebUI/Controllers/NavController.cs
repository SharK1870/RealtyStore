using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RealtyStore.Domain.Abstract;

namespace RealtyStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IRealtyRepository repository;

        public NavController(IRealtyRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Realties
                .Select(realty => realty.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("FlexMenu", categories);
        }
    }
}