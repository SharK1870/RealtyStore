using System.Linq;
using System.Web.Mvc;
using RealtyStore.Domain.Abstract;
using RealtyStore.Domain.Entities;
using RealtyStore.WebUI.Models;

namespace RealtyStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IRealtyRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IRealtyRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int realtyId, string returnUrl)
        {
            Realty realty = repository.Realties
                .FirstOrDefault(r => r.RealtyId == realtyId);

            if (realty != null)
            {
                cart.AddItem(realty, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int realtyId, string returnUrl)
        {
            Realty realty = repository.Realties
                .FirstOrDefault(r => r.RealtyId == realtyId);

            if (realty != null)
            {
                cart.RemoveLine(realty);
                return RedirectToAction("Index", new { returnUrl });
            }
            else {
                return null;
            }
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извените, Ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else { return View(shippingDetails); }
        }
    }
}