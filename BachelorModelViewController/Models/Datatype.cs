using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public abstract class Datatype
    {
        public string name { get { return name; } set { name = value; } }
        public Type  type { get; set; }

        public override string ToString()
        {
            return name + ":" + type.ToString();
        }
    }
}
