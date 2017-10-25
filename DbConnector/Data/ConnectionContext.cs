using DbConnector.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbConnector.Data
{
    public class ConnectionContext : DbContext
    {
        public ConnectionContext(DbContextOptions<ConnectionContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Users_UserGroups> Users_UserGroups{ get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Users_UserGroups>().ToTable("Users_UserGroups");
            modelBuilder.Entity<UserGroup>().ToTable("UserGroup");
        }
    }
}
