using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorModelViewController.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BachelorModelViewController.Models
{
    public class AccessRestriction
    {
        public int Id { get; set; }
        public bool GroupRestricted { get; set; }
        public bool UserRestricted { get; set; }
        public string AccessLevelId { get; set; }
        [ForeignKey("AccessLevelId")]
        public IdentityRole AccessLevel { get; set; }
    }
}
