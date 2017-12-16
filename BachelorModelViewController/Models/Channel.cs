using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorModelViewController.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BachelorModelViewController.Models
{
    public class Channel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
        public AccessRestriction AccessRestriction { get; set; }
        public DatatypeModel Content { get; set; }

    }
}
