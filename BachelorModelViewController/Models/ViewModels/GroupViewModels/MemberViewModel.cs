using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.GroupViewModels
{
    public class MemberViewModel
    {
        public User User { get; set; }
        public int GroupId { get; set; }
        public string RoleId { get; set; }
    }
}
