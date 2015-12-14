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
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty { RealtyId = 1, Name = "Недвижимость1"},
                new Realty { RealtyId = 2, Name = "Недвижимость2"},
                new Realty { RealtyId = 3, Name = "Недвижимость3"},
                new Realty { RealtyId = 4, Name = "Недвижимость4"},
                new Realty { RealtyId = 5, Name = "Недвижимость5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Realty> result = ((IEnumerable<Realty>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Недвижимость1", result[0].Name);
            Assert.AreEqual("Недвижимость2", result[1].Name);
            Assert.AreEqual("Недвижимость3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Game()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty { RealtyId = 1, Name = "Недвижимость1"},
                new Realty { RealtyId = 2, Name = "Недвижимость2"},
                new Realty { RealtyId = 3, Name = "Недвижимость3"},
                new Realty { RealtyId = 4, Name = "Недвижимость4"},
                new Realty { RealtyId = 5, Name = "Недвижимость5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Realty realty1 = controller.Edit(1).ViewData.Model as Realty;
            Realty realty2 = controller.Edit(2).ViewData.Model as Realty;
            Realty realty3 = controller.Edit(3).ViewData.Model as Realty;

            // Assert
            Assert.AreEqual(1, realty1.RealtyId);
            Assert.AreEqual(2, realty2.RealtyId);
            Assert.AreEqual(3, realty3.RealtyId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty { RealtyId = 1, Name = "Недвижимость1"},
                new Realty { RealtyId = 2, Name = "Недвижимость2"},
                new Realty { RealtyId = 3, Name = "Недвижимость3"},
                new Realty { RealtyId = 4, Name = "Недвижимость4"},
                new Realty { RealtyId = 5, Name = "Недвижимость5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Realty result = controller.Edit(6).ViewData.Model as Realty;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Realty
            Realty realty = new Realty { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(realty);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveRealty(realty));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Realty
            Realty realty = new Realty { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(realty);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveRealty(It.IsAny<Realty>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Games()
        {
            // Организация - создание объекта Realty
            Realty realty = new Realty { RealtyId = 1, Name = "Недвижимость1" };

            // Организация - создание имитированного хранилища данных
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty { RealtyId = 1, Name = "Недвижимость1" },
                new Realty { RealtyId = 2, Name = "Недвижимость2" },
                new Realty { RealtyId = 3, Name = "Недвижимость3" },
                new Realty { RealtyId = 4, Name = "Недвижимость4" },
                new Realty { RealtyId = 5, Name = "Недвижимость5" }
        });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление недвижимости
            controller.Delete(realty.RealtyId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Realty
            mock.Verify(m => m.DeleteRealty(realty.RealtyId));
        }
    }
}
