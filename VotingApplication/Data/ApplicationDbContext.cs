using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Models;

namespace VotingApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<User> // Make sure it's IdentityDbContext<User>
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
            base.OnModelCreating(modelBuilder);


            // Rename AspNetUsers to Users
            modelBuilder.Entity<User>()
                .ToTable("Users");

            // Set default values for UserRol and UserElection
            modelBuilder.Entity<User>()
                .Property(u => u.UserRol)
                .HasDefaultValue(2);

            modelBuilder.Entity<User>()
                .Property(u => u.UserElection)
                .HasDefaultValue(1);

            // Define relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.UserRol)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Election)
                .WithMany(e => e.Users)
                .HasForeignKey(u => u.UserElection)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.User)
                .WithMany(u => u.Candidates)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.Election)
                .WithMany(e => e.Candidates)
                .HasForeignKey(c => c.ElectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Candidate)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.VotesCandidate)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Election)
                .WithMany(e => e.Votes)
                .HasForeignKey(v => v.VotesElection)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
