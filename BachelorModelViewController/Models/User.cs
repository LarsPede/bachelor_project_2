using BachelorModelViewController.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BachelorModelViewController.Models
{
    public class User : IdentityUser
    {
        [Required]
        private Guid Token { get; set; }
        [Required]
        public string Name { get; set; }
        public Dictionary<Guid,int> Organization_AuthenticationLevel { get; set; }
    }
}
