using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models.Data
{
    public class ClassModel : DatatypeModel
    {
        public List<PropertyModel> properties{ get; set; }
    }
}
