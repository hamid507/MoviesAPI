using DataAccess.Abstractions;
using DataAccess.Abstractions.Repositories;
using Domain.Entities.Lookup;

namespace DataAccess.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
