using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BachelorModelViewController.Models;

namespace BachelorModelViewController.Data
{
    public class UserDb : IdentityDbContext<User>
    {
        public UserDb(DbContextOptions<UserDb> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>().ToTable("Users", "dbo");
            builder.Entity<User>().ToTable("Users", "dbo");
        }
    }
}
