
using Microsoft.EntityFrameworkCore;
using memoseeds.Models.Entities;

namespace memoseeds.Database
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Models.Entities.Type> Types { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Category> Courses { get; set; }
        public DbSet<Completion> Completions { get; set; }
        public DbSet<Collector> Collectors { get; set; }
        public DbSet<AquiredModules> AquiredModules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Term>()
                .HasOne(p => p.Module)
                .WithMany(t => t.Terms)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

