using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.GroupViewModels
{
    public class EditViewModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string GroupName { get; set; }
        public string Description { get; set; }
        public IEnumerable<MemberViewModel> Members { get; set; }
        public IEnumerable<MemberViewModel> ApplyingMembers { get; set; }
        public bool OneAdmin { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
