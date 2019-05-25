using System.Collections.Generic;

namespace IRunes.Models
{
    public class Album
    {

        public Album()
        {
            this.Tracks = new List<Track>();
        }
        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal Price { get; set; }

        public List<Track> Tracks;

        /*•	Id – a string (GuID).
           •	Name – a string.
           •	Cover – a string (a link to an image).
           •	Price – a decimal (sum of all Tracks’ prices, reduced by 13%).
           •	Tracks – a collection of Tracks.
           */
    }
}