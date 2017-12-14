using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BachelorModelViewController.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using BachelorModelViewController.Models.ViewModels.GroupViewModels;

namespace BachelorModelViewController.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // IdentityUser Mapping
            builder.Entity<User>().ToTable("Users", "dbo");
            builder.Entity<IdentityRole>().ToTable("Roles", "dbo");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // Mapping DbSets to dbo
            builder.Entity<Group>().ToTable("Groups", "dbo");
            builder.Entity<Association>().ToTable("Associations", "dbo");
            builder.Entity<Channel>().ToTable("Channels", "dbo");
            builder.Entity<AccessRestriction>().ToTable("AccessRestrictions", "dbo");

            // Set Unique

            builder.Entity<Group>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<Channel>()
                .HasIndex(u => u.Name)
                .IsUnique();

            // Additional OnModelCreating configurations
            builder.HasPostgresExtension("uuid-ossp");

        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Association> Associations { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<AccessRestriction> AccessRestrictions { get; set; }
    }
}
