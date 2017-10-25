using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BachelorModelViewController.Models.DataAccess
{
    public class Association : DbContext
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid UserGroup { get; set; }
        public Guid RoleId { get; set; }
    }
}
