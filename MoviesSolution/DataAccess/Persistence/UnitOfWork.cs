using DataAccess.Abstractions;
using DataAccess.Abstractions.Repositories;
using DataAccess.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace DataAccess.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbContext _dbContext;
        private bool _isDisposed = false;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork([NotNull] DbContextOptions options)
        {
            _dbContext = new InterviewDBContext(options);
            _dbContext.Database.Migrate();

            _repositories = new Dictionary<Type, object>();
        }

        public IUserRepository UserRepository => GetRepository<IUserRepository>();
        public IMovieRepository MovieRepository => GetRepository<IMovieRepository>();
        public IWatchItemRepository WatchItemRepository => GetRepository<IWatchItemRepository>();

        private TRepository GetRepository<TRepository>()
        {
            if (_repositories.Keys.Contains(typeof(TRepository)))
            {
                return (TRepository)_repositories[typeof(TRepository)];
            }

            var type = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(x => !x.IsAbstract && !x.IsInterface && x.Name == typeof(TRepository)
                .Name
                .Substring(1));

            if (type == null)
            {
                throw new Exception("Repository type is not found");
            }

            var repository = (TRepository)Activator.CreateInstance(type, _dbContext);
            _repositories.Add(typeof(TRepository), repository);

            return repository;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _dbContext.Dispose();
                _isDisposed = true;
            }
        }
    }
}
