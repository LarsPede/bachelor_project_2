using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorModelViewController.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BachelorModelViewController.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if(context.Roles.Any())
            {
                return; // DB has been seeded.
            }

            var roles = new IdentityRole[]
            {
                new IdentityRole{Name="God", NormalizedName="GOD"},
                new IdentityRole{Name="Administrator", NormalizedName="ADMINISTRATOR"},
                new IdentityRole{Name="Supplier", NormalizedName="SUPPLIER"},
                new IdentityRole{Name="Consumer", NormalizedName="CONSUMER"}
            };
            foreach (IdentityRole role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();
        }
    }
}
