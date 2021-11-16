using DataAccess.Abstractions;
using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Persistence.Configuration
{
    public class WatchItemConfig : DataEntityConfig<WatchItem>, IEntityConfig
    {
        public override void Configure(EntityTypeBuilder<WatchItem> builder)
        {
            base.Configure(builder);

            builder
                .HasKey(watchItem => new {watchItem.UserId, watchItem.MovieId });

            builder
                .HasOne(watchItem => watchItem.User)
                .WithMany()
                .HasForeignKey(watchItem => watchItem.UserId)
                .IsRequired();

            builder
                .HasOne(watchItem => watchItem.Movie)
                .WithMany()
                .HasForeignKey(watchItem => watchItem.MovieId)
                .IsRequired();
        }
    }
}
