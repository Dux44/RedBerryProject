using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    public class UserData:User
    {
        public long id_user { get; set; } 
        public string? firstname { get; set; } //усі інші поля при поданні заяви
        public string? secondname { get; set; } 
        public string? middlename { get; set; }
        public string? nationality { get; set; }
        public DateTime date_of_birth { get; set; }
        public string? address_of_birth { get; set; }
        public string? gender { get; set; }
        public string? addres_offical { get; set; }
        public string? addres_current { get; set; }
        public string? phone_number { get; set; }
        public string? card_number { get; set; }
        //public byte[]? TwoPhotosOfPassport { get; set; }
        
       
    }
}
