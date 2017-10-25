using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbConnector.Models
{
    public class UserGroup : Unit
    {
        public ICollection<User> Users { get; set; }
    }
}
