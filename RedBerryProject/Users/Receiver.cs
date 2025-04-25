using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Users
{
    internal class Receiver:User
    {
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        //else data to user
        public override string GetRole()
        {
            return "Receiver";
        }
    }
}
