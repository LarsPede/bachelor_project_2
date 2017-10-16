using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.models
{
    public class OrganizationOwnership : OwnershipBase
    {
        public string OrganizationName { get; set; }
        public Guid OrganizationId { get; set; }
        public Tuple<Guid, Guid> OrganizationKey { get; set; }
        
    }
}
