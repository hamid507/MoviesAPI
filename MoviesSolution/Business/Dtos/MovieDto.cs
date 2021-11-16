using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Dtos
{
    public class MovieDto
    {
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string ImdbRating { get; set; }
        public string PosterId { get; set; }
        public string PosterUrl { get; set; }
        public string WikiShortDescription { get; set; }
    }
}
