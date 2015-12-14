using System.Web.Mvc;
using System.Web;
using System.Linq;
using RealtyStore.Domain.Abstract;
using RealtyStore.Domain.Entities;

namespace RealtyStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IRealtyRepository repository;

        public AdminController(IRealtyRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Realties);
        }

        public ViewResult Edit(int realtyId)
        {
            Realty realty = repository.Realties
                .FirstOrDefault(r => r.RealtyId == realtyId);
            return View(realty);
        }

        [HttpPost]
        public ActionResult Edit(Realty realty)//, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                /*if (image != null)
                {
                    realty.ImageMineType = image.ContentType;
                    realty.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(realty.ImageData, 0, image.ContentLength);
                }*/
                repository.SaveRealty(realty);
                TempData["message"] = string.Format("Изменения в недвижимости \"{0}\" были сохранены", realty.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(realty);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Realty());
        }

        [HttpPost]
        public ActionResult Delete(int realtyId)
        {
            Realty deleteRealty = repository.DeleteRealty(realtyId);
            if (deleteRealty != null)
            {
                TempData["message"] = string.Format("Недвижимость \"{0}\" была удалена",
                    deleteRealty.Name);
            }
            return RedirectToAction("Index");
        }
    }
}