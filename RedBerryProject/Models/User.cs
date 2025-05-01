using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    public  class User
    {
        public long Id { get; set; } // це буде отримуватись під час завантаження з БД
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        //public string? Name { get; set; }
        //public string? Surname { get; set; }
        //public string? MiddleName { get; set; }
        //public abstract string GetRole();
    }
}
