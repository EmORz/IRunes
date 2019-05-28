using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;

namespace IRunes.App.Controllers
{
    public class UsersController: Controller
    {
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
        public IHttpResponse Login(IHttpRequest httpRequest)
        {
            return this.View();
        }
        [HttpPost(ActionName = "Login")]
        public IHttpResponse LoginConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                var username = ((ISet<string>)(httpRequest.FormData["username"])).FirstOrDefault();
                var password = ((ISet<string>)(httpRequest.FormData["password"])).FirstOrDefault();

                User userFromDb = context.Users.FirstOrDefault(user => user.Username == username
                                                                       || user.Email == username
                                                                       && user.Password == this.HashPassword(password)
                                                                       );
                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }
                this.SignIn(httpRequest, userFromDb.Id, userFromDb.Username, userFromDb.Email);
            }
            return Redirect("/");
        }
        public IHttpResponse Register(IHttpRequest httpRequest)
        {
            return this.View();
        }
        [HttpPost(ActionName = "Register")]
        public IHttpResponse RegisterConfirm(IHttpRequest httpRequest)
        {
            using (var context = new RunesDbContext())
            {
                var username = ((ISet<string>)(httpRequest.FormData["username"])).FirstOrDefault();
                var password = ((ISet<string>)(httpRequest.FormData["password"])).FirstOrDefault();
                var confirmPassword = ((ISet<string>)(httpRequest.FormData["confirmPassword"])).FirstOrDefault();
                var email = ((ISet<string>)(httpRequest.FormData["email"])).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("/Users/Register");
                }

                User user = new User
                {
                    Username = username,
                    Password = this.HashPassword(password),
                    Email = email
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
            return this.Redirect("/Users/Login");
        }

        public IHttpResponse Logout(IHttpRequest httpRequest)
        {
            //httpRequest.Session.ClearParameters();
            this.SignOut(httpRequest);
            return Redirect("/");
        }

    }
}