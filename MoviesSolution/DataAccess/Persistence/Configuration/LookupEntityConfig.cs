using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DataAccess.Persistence.Configuration
{
    public class LookupEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : LookupEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).ValueGeneratedOnAdd().HasValueGenerator<GuidValueGenerator>();

            var entityName = typeof(TEntity).Name;
            builder.ToTable(entityName, "dbo");
        }
    }
}