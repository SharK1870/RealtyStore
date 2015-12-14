using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealtyStore.Domain.Abstract;
using RealtyStore.Domain.Entities;
using RealtyStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace RealtyStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        /*
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
           Realty realty = new Realty
            {
                RealtyId = 2,
                Name = "Недвижимость2",
                //ImageData = new byte[] { },
                //ImageMineType = "image/jpg"
            };

            // Организация - создание имитированного хранилища
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty> {
                new Realty {RealtyId = 1, Name = "Недвижимость1"},
                realty,
                new Realty {RealtyId = 3, Name = "Недвижимость3"}
            }.AsQueryable());

            // Организация - создание контроллера
            RealtyController controller = new RealtyController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(realty.ImageMineType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IRealtyRepository> mock = new Mock<IRealtyRepository>();
            mock.Setup(m => m.Realties).Returns(new List<Realty> {
                new Realty {RealtyId = 1, Name = "Недвижимость1"},
                new Realty {RealtyId = 2, Name = "Недвижимость2"}
            }.AsQueryable());

            // Организация - создание контроллера
            RealtyController controller = new RealtyController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }*/
    }
}