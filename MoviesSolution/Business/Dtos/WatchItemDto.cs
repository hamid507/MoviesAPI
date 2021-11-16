using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Dtos
{
    public class WatchItemDto
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
    }
}
