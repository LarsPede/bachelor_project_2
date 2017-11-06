using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public class Atomic : Datatype
    {
        public string value { get; set; }
        public override string ToString()
        {
            return "\"" + name + "\":\"" + type;
        }

        public Atomic(string n, Type t)
        {
            name = n;
            type = t;
        }
        
    }
}
