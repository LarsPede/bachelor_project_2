using BachelorModelViewController.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public class Channel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> ColoumnDiscription { get; set; }
        //public OwnershipBase Ownership { get; set; }
    }
}
