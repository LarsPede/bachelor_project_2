using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.models.LoginInformation
{
    public class LoginBase
    {
        private Tuple<Guid, Guid> Login { get; set; }
        public string Email { get; set; }
    }
}
