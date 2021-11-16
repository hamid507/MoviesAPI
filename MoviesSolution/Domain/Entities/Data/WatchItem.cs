using Domain.Entities.Lookup;
using System;

namespace Domain.Entities.Data
{
    public class WatchItem : DataEntity
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public bool Watched { get; set; }
        public DateTime? LastEmailSendDate { get; set; }

        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}
