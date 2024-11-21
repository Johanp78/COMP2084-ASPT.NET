using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VotingApplication.Models;

namespace VotingApplication.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User -> Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.UserRol)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Election
            modelBuilder.Entity<User>()
                .HasOne(u => u.Election)
                .WithMany(e => e.Users)
                .HasForeignKey(u => u.UserElection)
                .OnDelete(DeleteBehavior.Restrict);

            // Candidate -> User
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.User)
                .WithMany(u => u.Candidates)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Candidate -> Election
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.Election)
                .WithMany(e => e.Candidates)
                .HasForeignKey(c => c.ElectionElection)
                .OnDelete(DeleteBehavior.Restrict);

            // Vote -> Candidate
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Candidate)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.VotesCandidate)
                .OnDelete(DeleteBehavior.Restrict);

            // Vote -> Election
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Election)
                .WithMany(e => e.Votes)
                .HasForeignKey(v => v.VotesElection)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
