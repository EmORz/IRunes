using System.Collections.Generic;
using System.Linq;
using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers
{
    public class AlbumsController : BaseController
    {
        public IHttpResponse All(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                this.Redirect("/Users/Login");
            }
            using (var context = new RunesDbContext())
            {
                ICollection<Album> allAlbums = context.Albums.ToList();
                if (allAlbums.Count == 0)
                {
                    this.ViewData["Albums"] = "There are currently no albums.";
                }
                else
                {

                    this.ViewData["Albums"] =
                        string.Join("<br />",
                        allAlbums.Select(album => album.ToHtmlAll()).ToList());
                }

            }
            return this.View();

        }

        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                this.Redirect("/Users/Login");
            }
            return this.View();
        }
        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                this.Redirect("/Users/Login");
            }

            using (var context = new RunesDbContext())
            {
                var name = ((ISet<string>)httpRequest.FormData["name"]).FirstOrDefault();
                var cover = ((ISet<string>)httpRequest.FormData["cover"]).FirstOrDefault();

                Album album = new Album
                {
                    Name =  name,
                    Cover = cover,
                    Price = 0M
                };
                context.Albums.Add(album);
                context.SaveChanges();
            }
            return this.Redirect("/Albums/All");
        }

        public IHttpResponse Details(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                this.Redirect("/Users/Login");
            }

            var albumId = httpRequest.FormData["id"].ToString();
            using (var context = new RunesDbContext())
            {
                var albumFromDB = context.Albums.SingleOrDefault(album => album.Id == albumId);
                if (albumFromDB == null)
                {
                    return Redirect("/Albums/All");
                }

                this.ViewData["Album"] = albumFromDB.ToHtmlDetails();
                return this.View(); 

            }
            return this.View();
        }

    }
}