using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    public class Admin:User
    {
        public long IdUser { get; set; }                 // @IdUser
        public string? FirstName { get; set; }          // @FirstName
        public string? SecondName { get; set; }          // @SecondName
        public string? MiddleName { get; set; }          // @MiddleName
        public int IdHelpPoint { get; set; }             // @IdHelpPoint

    }
}
