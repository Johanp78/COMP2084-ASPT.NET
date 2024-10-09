using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Models;

namespace VotingApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<VotingApplication.Models.Election> Election { get; set; } = default!;
        public DbSet<VotingApplication.Models.Vote> Vote { get; set; } = default!;
    }
}
