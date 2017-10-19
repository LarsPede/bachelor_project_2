using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.models
{
    public class Organization
    {
        public Guid ID { get; set; }
        public Dictionary<Guid, int> User_AuthenticationLevel { get; set; }

    }
}
