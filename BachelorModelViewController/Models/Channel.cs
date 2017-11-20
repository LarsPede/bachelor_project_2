using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BachelorModelViewController.Models.Data;

namespace BachelorModelViewController.Models
{
    public class Channel
    {
        public Guid Owner { get; set; }
        public Guid Group { get; set; }
        public DatatypeModel Content { get; set; }

        public string getDoc()
        {
            return Content.ToString();
        }

    }
}
