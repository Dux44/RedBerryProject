using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    public class Receiver:User
    {
        public long Id_user { get; set; } 
        public string? FirstName { get; set; } //усі інші поля при поданні заяви
        public string? Surname { get; set; } 
        public string? MiddleName { get; set; }
        public string? Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? AddresOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? AddresOffical { get; set; }
        public string? AddresCurrent { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CardNumber { get; set; }
        //public byte[]? TwoPhotosOfPassport { get; set; }
        
       
    }
}
