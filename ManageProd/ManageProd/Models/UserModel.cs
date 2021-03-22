using System;
using System.Collections.Generic;
using System.Text;

namespace ManageProd.Models
{
    public class UserModel
    {
        public string User { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
