using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Abstractions
{
    public interface IDbContext
    {
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
        int SaveChanges();
        void Dispose();
    }
}
