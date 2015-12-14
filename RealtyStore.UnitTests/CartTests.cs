using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using RealtyStore.Domain.Entities;
using RealtyStore.Domain.Abstract;
using Moq;
using RealtyStore.WebUI.Controllers;
using System.Web.Mvc;
using RealtyStore.WebUI.Models;

namespace RealtyStore.UnitTests
{
    [TestClass]
    public class CartTests
    {

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание тестовой недвижимости
            Realty realty1 = new Realty { RealtyId = 1, Name = "Недвижимость1" };
            Realty realty2 = new Realty { RealtyId = 2, Name = "Недвижимость2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(realty1, 1);
            cart.AddItem(realty2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Realty, realty1);
            Assert.AreEqual(results[1].Realty, realty2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание тестовой недвижимости
            Realty realty1 = new Realty { RealtyId = 1, Name = "Недвижимость1" };
            Realty realty2 = new Realty { RealtyId = 2, Name = "Недвижимость2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(realty1, 1);
            cart.AddItem(realty2, 1);
            cart.AddItem(realty1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Realty.RealtyId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание тестовой недвижимости
            Realty realty1 = new Realty { RealtyId = 1, Name = "Недвижимость1" };
            Realty realty2 = new Realty { RealtyId = 2, Name = "Недвижимость2" };
            Realty realty3 = new Realty { RealtyId = 3, Name = "Недвижимость3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(realty1, 1);
            cart.AddItem(realty2, 4);
            cart.AddItem(realty3, 2);
            cart.AddItem(realty2, 1);

            // Действие
            cart.RemoveLine(realty2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Realty == realty2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание тестовой недвижимости
            Realty realty1 = new Realty { RealtyId = 1, Name = "Недвижимость1", Price = 100 };
            Realty realty2 = new Realty { RealtyId = 2, Name = "Недвижимость2", Price = 50 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(realty1, 1);
            cart.AddItem(realty2, 1);
            cart.AddItem(realty1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 650);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание тестовой недвижимости
            Realty realty1 = new Realty { RealtyId = 1, Name = "Недвижимость1", Price = 100 };
            Realty realty2 = new Realty { RealtyId = 2, Name = "Недвижимость2", Price = 50 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(realty1, 1);
            cart.AddItem(realty2, 1);
            cart.AddItem(realty1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        /// <summary>
        /// Проверяем добавление в корзину
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1,Name="Недвижимость1", Category="Cat1" },
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Realty.RealtyId, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Realty_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1,Name="Недвижимость1", Category="Cat1" },
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null, null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Realty(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Realty(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
