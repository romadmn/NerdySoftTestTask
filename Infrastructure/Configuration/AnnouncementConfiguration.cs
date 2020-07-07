using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("Announcement");
            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasColumnName("title")
                .HasMaxLength(50);

            builder.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(1000);

            builder.Property(a => a.DateAdded)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
