using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public class Class : Datatype
    {
        public Dictionary<int, Datatype> content { get; set; }

        public Class(Dictionary<int, Datatype> d)
        {
            content = d;
        }

        public override string ToString()
        {
            string temp = "\"" + name + "\": {";

            foreach (var key in content.Keys)
            {
                temp += content[key].ToString() + ",";
            }

            temp.Remove(temp.LastIndexOf(","));

            temp += "},";

            return temp;
        }
    }
}
