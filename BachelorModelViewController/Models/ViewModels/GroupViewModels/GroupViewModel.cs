using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BachelorModelViewController.Models.ViewModels.GroupViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public IdentityRole Role { get; set; }
    }
}
