using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.ChannelViewModels
{
    public class DetailsViewModel
    {
        public ChannelViewModel Channel { get; set; }
        public bool EditAccess { get; set; }
    }
}
