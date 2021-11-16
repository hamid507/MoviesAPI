using DataAccess.Abstractions;
using DataAccess.Abstractions.Repositories;
using Domain.Entities.Lookup;

namespace DataAccess.Persistence.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
