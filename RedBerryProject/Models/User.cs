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
        public long id { get; set; } // це буде отримуватись під час завантаження з БД
        public string? username { get; set; }
        public string? password { get; set; }
        public string? role { get; set; }
       
    }
}
