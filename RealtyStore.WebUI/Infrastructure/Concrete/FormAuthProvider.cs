using System.Web.Security;
using RealtyStore.WebUI.Infrastructure.Abstract;

namespace RealtyStore.WebUI.Infrastructure.Concrete
{
    public class FormAuthProvider : IAuthProvider
    {
        bool IAuthProvider.Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if (result) FormsAuthentication.SetAuthCookie(username, false);
            return result;
        }
    }
}