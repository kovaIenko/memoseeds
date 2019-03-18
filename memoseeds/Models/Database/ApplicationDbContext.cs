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
     
    }
    }

