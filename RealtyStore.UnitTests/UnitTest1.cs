using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealtyStore.Domain.Abstract;
using RealtyStore.Domain.Entities;
using RealtyStore.WebUI.Controllers;
using RealtyStore.WebUI.Models;
using RealtyStore.WebUI.HtmlHelpers;

namespace RealtyStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1, Name="Недвижимость1" },
                new Realty {RealtyId=2, Name="Недвижимость2" },
                new Realty {RealtyId=3, Name="Недвижимость3" },
                new Realty {RealtyId=4, Name="Недвижимость4" },
                new Realty {RealtyId=5, Name="Недвижимость5" }
            });
            RealtyController controller = new RealtyController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            RealtiesListViewModel result = (RealtiesListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Realty> realties = result.Realties.ToList();
            Assert.IsTrue(realties.Count == 2);
            Assert.AreEqual(realties[0].Name, "Недвижимость4");
            Assert.AreEqual(realties[1].Name, "Недвижимость5");
        }

        [TestMethod]
        public void Can_Genetate_Page_Links()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        // ...
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1,Name="Недвижимость1" },
                new Realty {RealtyId=2,Name="Недвижимость2" },
                new Realty {RealtyId=3,Name="Недвижимость3" },
                new Realty {RealtyId=4,Name="Недвижимость4" },
                new Realty {RealtyId=5,Name="Недвижимость5" }
            });
            RealtyController controller = new RealtyController(mock.Object);
            controller.pageSize = 3;

            // Act
            RealtiesListViewModel result = (RealtiesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Realty()
        {
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1, Name="Недвижимость1", Category = "Cat1" },
                new Realty {RealtyId=2, Name="Недвижимость2", Category = "Cat2" },
                new Realty {RealtyId=3, Name="Недвижимость3", Category = "Cat1" },
                new Realty {RealtyId=4, Name="Недвижимость4", Category = "Cat2" },
                new Realty {RealtyId=5, Name="Недвижимость5", Category = "Cat3" }
            });
            RealtyController controller = new RealtyController(mock.Object);
            controller.pageSize = 3;

            // Действие
            List<Realty> result = ((RealtiesListViewModel)controller.List("Cat2", 1).Model).Realties.ToList();

            // Утверждение (assert)
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Недвижимость2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Недвижимость4" && result[0].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1, Name="Недвижимость1", Category = "Дача" },
                new Realty {RealtyId=2, Name="Недвижимость2", Category = "Дача" },
                new Realty {RealtyId=3, Name="Недвижимость3", Category = "Дом" },
                new Realty {RealtyId=4, Name="Недвижимость4", Category = "Квартира" }
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Дача");
            Assert.AreEqual(results[1], "Дом");
            Assert.AreEqual(results[2], "Квартира");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1, Name="Недвижимость1", Category = "Дача" },
                new Realty {RealtyId=2, Name="Недвижимость2", Category = "Дом" }
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Дача";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Realty_Count()
        {
            /// Организация (arrange)
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty>
            {
                new Realty {RealtyId=1, Name="Недвижимость1", Category = "Cat1" },
                new Realty {RealtyId=2, Name="Недвижимость2", Category = "Cat2" },
                new Realty {RealtyId=3, Name="Недвижимость3", Category = "Cat1" },
                new Realty {RealtyId=4, Name="Недвижимость4", Category = "Cat2" },
                new Realty {RealtyId=5, Name="Недвижимость5", Category = "Cat3" }
            });
            RealtyController controller = new RealtyController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((RealtiesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((RealtiesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((RealtiesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((RealtiesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
