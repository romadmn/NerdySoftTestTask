using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class NerdySoftContext : DbContext
    {
        public NerdySoftContext(DbContextOptions<NerdySoftContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Announcement> Announcement { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AnnouncementConfiguration());
        }
    }
}
