using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public class Composite : Datatype
    {
        List<Datatype> list { get ; set; }

        public Composite(List<Datatype> l)
        {
            list = l;
        }

        public override string ToString()
        {
            string s = "\""+ name + ":[";

            if (list.Count == 1)
            {
                s += list[0].ToString();
            }
            else
            {
                foreach (var item in list)
                {
                    s += "{" + item.ToString() + "},";
                }
            }

            s.Remove(s.LastIndexOf(","));
            s += "]";

            return s;
        }
    }
}
