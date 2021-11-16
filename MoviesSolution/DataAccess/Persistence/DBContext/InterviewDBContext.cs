using DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace DataAccess.Persistence.DBContext
{
    public partial class InterviewDBContext : DbContext, IDbContext
    {
        private readonly Dictionary<Type, object> _dbSets;

        public InterviewDBContext([NotNull] DbContextOptions options) : base(options)
        {
            _dbSets = new Dictionary<Type, object>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
                x => !x.IsAbstract && !x.IsInterface && typeof(IEntityConfig).IsAssignableFrom(x));

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            if (_dbSets.Keys.Contains(typeof(TEntity)))
            {
                return _dbSets[typeof(TEntity)] as DbSet<TEntity>;
            }

            var dbSet = Set<TEntity>();
            _dbSets.Add(typeof(TEntity), dbSet);

            return dbSet;
        }
    }
}
