using DataAccess.Abstractions;
using DataAccess.Abstractions.Repositories;
using Domain.Entities.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Persistence.Repositories
{
    public class WatchItemRepository : GenericRepository<WatchItem>, IWatchItemRepository
    {
        public WatchItemRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
