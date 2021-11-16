using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DataAccess.Persistence.Configuration
{
    public class DataEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : DataEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            //builder.HasNoKey();
        }
    }
}