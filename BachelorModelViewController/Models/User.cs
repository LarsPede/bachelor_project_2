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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public override string Email { get; set; }
    }
}
