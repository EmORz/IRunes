using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using IRunes.Models;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Result;

namespace IRunes.App.Controllers
{
    public abstract class BaseController
    {
        protected void SignIn(IHttpRequest request, User user)
        {
            request.Session.AddParameter("id", user.Id);
            request.Session.AddParameter("username", user.Username);
            request.Session.AddParameter("email", user.Email);
        }

        protected void SignOut(IHttpRequest request)
        {
            request.Session.ClearParameters();
        }

        protected Dictionary<string, object> ViewData;

        protected BaseController()
        {
            this.ViewData = new Dictionary<string, object>();
        }
        protected bool IsLoggedIn(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        protected string ParseTemplate(string viewContent)
        {
            foreach (var param in this.ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }

            return viewContent;
        }

        protected IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            viewContent = this.ParseTemplate(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return htmlResult;
        }

        protected IHttpResponse Redirect(string url)
        {
            return new RedirectResult(url);
        }
    }
}