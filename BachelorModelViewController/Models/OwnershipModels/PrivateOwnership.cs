using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.models.OwnershipModels
{
    public class PrivateOwnership : OwnershipBase
    {
        public Guid OwnerId { get; set; }
        public Tuple<Guid,Guid> PrivateKey { get; set; }
    }
}
