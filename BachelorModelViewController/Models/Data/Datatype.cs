using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public class Datatype
    {
        public string name { get; set; }
        public Type  type { get; set; }

        public Datatype() { }

        public override string ToString()
        {
            return name + ":" + type.ToString();
        }

        public void createFromString(string s)
        {
            name = "DataStructure";
            
        }
    }
}
