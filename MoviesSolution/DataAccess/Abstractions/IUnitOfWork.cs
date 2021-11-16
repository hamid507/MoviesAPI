using DataAccess.Abstractions.Repositories;

namespace DataAccess.Abstractions
{
    public interface IUnitOfWork
    {
        void Commit();

        IMovieRepository MovieRepository { get; }
        IUserRepository UserRepository { get; }
        IWatchItemRepository WatchItemRepository { get; }
    }
}
