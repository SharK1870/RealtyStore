using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealtyStore.Domain.Abstract;
using RealtyStore.Domain.Entities;
using RealtyStore.WebUI.Models;

namespace RealtyStore.WebUI.Controllers
{
    public class RealtyController : Controller
    {
        private IRealtyRepository repository;
        public int pageSize = 3;

        public RealtyController(IRealtyRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            RealtiesListViewModel model = new RealtiesListViewModel
            {
                Realties = repository.Realties
                .Where(p => category == null || p.Category == category)
                .OrderBy(realties => realties.RealtyId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? repository.Realties.Count() : 
                    repository.Realties.Where(reality => reality.Category == category).Count()
                },
                CurrentCategory=category
            };
            return View(model);
        }
        /*
        public FileContentResult GetImage(int realtyId)
        {
            Realty realty = repository.Realties
                .FirstOrDefault(r => r.RealtyId == realtyId);

            if (realty != null)
            {
                return File(realty.ImageData, realty.ImageMineType);
            }
            else
            {
                return null;
            }
        }*/
    }
}