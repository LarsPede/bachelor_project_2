using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.ViewModels.SharedViewModels
{
    public class AccessRestrictionViewModel
    {

        public int Id { get; set; }
        public bool GroupRestricted { get; set; }
        public bool UserRestricted { get; set; }
        public string AccessLevelId { get; set; }
        [ForeignKey("AccessLevelId")]
        public IdentityRole AccessLevel { get; set; }

        public static implicit operator AccessRestrictionViewModel(AccessRestriction accessRestriction)
        {
            return new AccessRestrictionViewModel
            {
                Id = accessRestriction.Id,
                GroupRestricted = accessRestriction.GroupRestricted,
                UserRestricted = accessRestriction.UserRestricted,
                AccessLevelId = accessRestriction.AccessLevelId,
                AccessLevel = accessRestriction.AccessLevel
            };
        }

        public static implicit operator AccessRestriction(AccessRestrictionViewModel vm)
        {
            return new AccessRestriction
            {
                Id = vm.Id,
                GroupRestricted = vm.GroupRestricted,
                UserRestricted = vm.UserRestricted,
                AccessLevelId = vm.AccessLevelId,
                AccessLevel = vm.AccessLevel
            };
        }
    }
}
