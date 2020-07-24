using Microsoft.EntityFrameworkCore;
using Polls.API.Entities;

namespace Polls.API.DbContexts
{
    public class PollsContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }
        
        public PollsContext(DbContextOptions<PollsContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Options)
                .WithOne(o => o.Poll)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Votes)
                .WithOne(v => v.Poll)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Poll>()
                .HasIndex(p => p.Slug)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}