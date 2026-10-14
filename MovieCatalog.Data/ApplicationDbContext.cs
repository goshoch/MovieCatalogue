using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Entities;

namespace MovieCatalog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserReview>().HasKey(x=>new {x.UserId,x.MovieId});
            modelBuilder.Entity<UserReview>().HasOne(x=>x.User).WithMany(x=>x.UserReviews)
                .HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserReview>().HasOne(x=>x.Movie).WithMany(x=>x.UserReviews)
                .HasForeignKey(x=>x.MovieId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}