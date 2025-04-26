using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    public abstract class User
    {
        public int ID { get; set; }
        public string UserName { get; set; } = "";
        public abstract string GetRole();
    }
}
