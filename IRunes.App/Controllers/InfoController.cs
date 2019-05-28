using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {

        public IHttpResponse About(IHttpRequest request)
        {
            //todo implement view diseign
            return View();

        }
    }
}