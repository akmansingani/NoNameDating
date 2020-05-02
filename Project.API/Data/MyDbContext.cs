using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.API.Models;


namespace Project.API.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        { }

        public DbSet<TestTable> TestTable { get; set; }

        public DbSet<Users> Users { get; set; }

        // public DbSet<Photos> Photos { get; set; }

        public DbSet<Likes> Likes { get; set; }

        public DbSet<Messages> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Likes>()
                .HasKey(q => new { q.LikeByUserID, q.LikedUserID });

            modelBuilder.Entity<Likes>()
               .HasOne(u => u.LikedUser)
               .WithMany(u => u.LikeByUsers)
               .HasForeignKey(u => u.LikedUserID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Likes>()
                .HasOne(u => u.LikeByUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(u => u.LikeByUserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Messages>()
                 .HasOne(u => u.Sender)
                 .WithMany(u => u.MessagesSent)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Messages>()
                .HasOne(u => u.Receiver)
                .WithMany(u => u.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
