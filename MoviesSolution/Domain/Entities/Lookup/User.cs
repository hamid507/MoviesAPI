using Domain.Entities.Data;
using System.Collections.Generic;

namespace Domain.Entities.Lookup
{
    public class User : LookupEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}