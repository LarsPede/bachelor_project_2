using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.ChannelViewModels
{
    public class AccessibleChannelsViewModel
    {
        public IEnumerable<ChannelViewModel> UnRestrictedChannels { get; set; }
        public IEnumerable<ChannelViewModel> UserRestrictedChannels { get; set; }
        public IEnumerable<ChannelViewModel> GroupRestrictedChannels { get; set; }
    }
}
