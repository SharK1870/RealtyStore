using System.Web.Mvc;
using System.Web.Routing;
using RealtyStore.Domain.Entities;
using RealtyStore.WebUI.Infrastructure.Binders;

namespace RealtyStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());

        }
    }
}
