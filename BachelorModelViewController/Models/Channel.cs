using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public class Channel
    {
        public Guid Owner { get; set; }
        public Guid Group { get; set; }
        public Datatype Content { get; set; }

        public string getDoc()
        {
            return Content.ToString();
        }

    }
}
