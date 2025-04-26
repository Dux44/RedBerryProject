using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    internal class Admin:User
    {
        private int Id_HelpPoint;
        public override string GetRole()
        {
            return "Admin";
        }
    }
}
