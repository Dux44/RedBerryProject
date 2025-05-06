using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    public class Admin:User
    {
        public long id_user { get; set; }                
        public string? firstname { get; set; }          
        public string? secondname { get; set; }          
        public string? middlename { get; set; }         
        public int id_help_point { get; set; }            

    }
}
