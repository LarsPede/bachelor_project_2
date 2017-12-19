using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.ChannelViewModels
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
        public int AccessRestriction { get; set; }
        public string JsonContentAsString { get; set; }
    }
}
