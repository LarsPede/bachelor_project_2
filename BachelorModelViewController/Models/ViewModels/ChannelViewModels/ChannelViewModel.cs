using BachelorModelViewController.Models.Data;
using BachelorModelViewController.Models.ViewModels.SharedViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.ChannelViewModels
{
    public class ChannelViewModel
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
        public int? DaysRestriction { get; set; }
        public int AccessRestrictionId { get; set; }
        [ForeignKey("AccessRestrictionId")]
        public AccessRestrictionViewModel AccessRestriction { get; set; }

        public static implicit operator ChannelViewModel(Channel channel)
        {
            return new ChannelViewModel
            {
                Id = channel.Id,
                Name = channel.Name,
                Description = channel.Description,
                UserId = channel.UserId,
                User = channel.User,
                GroupId = channel.GroupId,
                Group = channel.Group,
                DaysRestriction = channel.DaysRestriction,
                AccessRestrictionId = channel.AccessRestrictionId,
                AccessRestriction = channel.AccessRestriction
            };
        }

        public static implicit operator Channel(ChannelViewModel vm)
        {
            return new Channel
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                UserId = vm.UserId,
                User = vm.User,
                GroupId = vm.GroupId,
                Group = vm.Group,
                DaysRestriction = vm.DaysRestriction,
                AccessRestrictionId = vm.AccessRestrictionId,
                AccessRestriction = vm.AccessRestriction
            };
        }
    }
}
