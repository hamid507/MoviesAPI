using DataAccess.Abstractions;
using DataAccess.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        protected readonly IDbContext _dbContext;

        public GenericRepository(IDbContext IDbContext)
        {
            _dbSet = IDbContext.GetDbSet<TEntity>();
            _dbContext = IDbContext;
        }

        private IQueryable<TEntity> AsQueryable() => _dbSet.AsQueryable();

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public bool Any()
        {
            return _dbSet.Any();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _dbSet.AsQueryable();

            var result = query.FirstOrDefault(predicate);

            return result;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            var query = AsQueryable();
            var result = query.Where(predicate);

            return result;
        }

        public IQueryable<TEntity> GetAll()
        {
            var query = AsQueryable();

            return query;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}
