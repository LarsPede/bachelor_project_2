using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorModelViewController.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BachelorModelViewController.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
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

            if (!context.AccessRestrictions.Any())
            {
                var accessRestrictions = new AccessRestriction[]
                {
                new AccessRestriction{GroupRestricted=false, UserRestricted=false},
                new AccessRestriction{GroupRestricted=false, UserRestricted=true},
                new AccessRestriction{GroupRestricted=true, UserRestricted=false},
                new AccessRestriction{GroupRestricted=true, UserRestricted=false, AccessLevel=roleManager.FindByNameAsync("Supplier").Result},
                new AccessRestriction{GroupRestricted=true, UserRestricted=false, AccessLevel=roleManager.FindByNameAsync("Administrator").Result}
                };
                foreach (AccessRestriction accessRestriction in accessRestrictions)
                {
                    context.AccessRestrictions.Add(accessRestriction);
                }
                context.SaveChanges();
            }
        }
    }
}
