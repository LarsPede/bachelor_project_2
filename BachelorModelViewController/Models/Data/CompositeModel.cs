using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.Data
{
    public class CompositeModel : DatatypeModel
    {
        public DatatypeModel datatypeModel { get; set; }
    }
}
