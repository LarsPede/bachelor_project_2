using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.AccessViewModel
{
    public class AccessCredentialViewModel
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public IdentityRole Role { get; set; }
        public bool groupDemanded { get; set; }
        public bool userDemanded { get; set; }

    }
}
