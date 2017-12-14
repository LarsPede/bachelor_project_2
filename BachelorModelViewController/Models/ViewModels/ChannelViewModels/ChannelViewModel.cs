using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.ChannelViewModels
{
    public class ChannelViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
        public string EntryPoint { get; set; }
        public string EndPoint { get; set; }

        public static implicit operator ChannelViewModel(Channel channel)
        {
            return new ChannelViewModel
            {
                Name = channel.Name,
                Description = channel.Description,
                User = channel.User,
                Group = channel.Group,
                EntryPoint = channel.EntryPoint,
                EndPoint = channel.EndPoint
            };
        }

        public static implicit operator Channel(ChannelViewModel vm)
        {
            return new Channel
            {
                Name = vm.Name,
                Description = vm.Description,
                User = vm.User,
                Group = vm.Group,
                EntryPoint = vm.EntryPoint,
                EndPoint = vm.EndPoint
            };
        }
    }
}
