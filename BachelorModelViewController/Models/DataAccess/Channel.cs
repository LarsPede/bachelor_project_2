using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BachelorModelViewController.Models.DataAccess
{
    public class Channel : DbContext
    {
        public List<Guid> AccessIn { get; set; }
        public List<Guid> AccessOut { get; set; }
        public string EntryPoint { get; set; } // path in
        public string EndPoint { get; set; } // path out

    }
}
