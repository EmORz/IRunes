using System.Collections.Generic;
using System.IO;
using System.Linq;
using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            var albumId = httpRequest.FormData["albumId"].ToString();

            this.ViewData["AlbumId"] = albumId;

            return this.View();
        }

        [HttpPost(ActionName = "Create")]
        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }
            var albumId = httpRequest.FormData["albumId"].ToString();
            using (var context = new RunesDbContext())
            {
                var albumFromDb = context.Albums.SingleOrDefault(album => album.Id == albumId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

                var name = ((ISet<string>)(httpRequest.FormData["name"])).FirstOrDefault();
                var link = ((ISet<string>)(httpRequest.FormData["link"])).FirstOrDefault();
                var price = ((ISet<string>)(httpRequest.FormData["price"])).FirstOrDefault();

                Track trackForDb =  new Track
                {
                    Name =  name,
                    Link = link,
                    Price = decimal.Parse(price)
                };
                albumFromDb.Tracks.Add(trackForDb);
                albumFromDb.Price = (albumFromDb.Tracks.Select(track => track.Price).Sum()*87)/100;
                context.Update(albumFromDb);
                context.SaveChanges();
            }
            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            var trackId = httpRequest.QueryData["trackId"].ToString();
            var albumId = httpRequest.QueryData["albumId"].ToString();
            using (var context = new RunesDbContext())
            {
                var trackFromDb = context.Tracks.SingleOrDefault(track => track.Id == trackId);
                if (trackFromDb == null)
                {
                    return Redirect($"/Albums/Details?id={albumId}");
                }

                this.ViewData["AlbumId"] = albumId;
                this.ViewData["Track"] = trackFromDb.ToHtmlDetails(albumId);
                return this.View();

            }
        }
        
    }
}