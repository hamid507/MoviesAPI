using Domain.Entities.Data;
using System.Collections.Generic;

namespace Domain.Entities.Lookup
{
    public class Movie : LookupEntity
    {
        public string MovieId { get; set; }
        public string Title { get; set; }
        public double ImdbRating { get; set; }
        public string PosterId { get; set; }
        public string PosterUrl { get; set; }
        public string WikiShortDescription { get; set; }
    }
}