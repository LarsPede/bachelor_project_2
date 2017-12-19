using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.ChannelViewModels
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool EditAccess { get; set; }
    }
}
