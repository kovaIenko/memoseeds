using System;
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
        public DbSet<Term> Terms { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Category> Courses { get; set; }
        public DbSet<Completion> Completions { get; set; }
        public DbSet<Collector> Collectors { get; set; }
    }
}

