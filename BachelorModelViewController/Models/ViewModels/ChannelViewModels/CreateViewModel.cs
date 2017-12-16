using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.ChannelViewModels
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
        public IEnumerable<Group> AccessibleGroups { get; set; }
    }
}
