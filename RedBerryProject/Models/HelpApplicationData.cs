using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBerryProject.Models
{
    public class HelpApplicationData
    {
        public long id { get; set; }
        public long id_user_data { get; set; }
        public int id_help_point { get; set; }
        public string? message_why_ran_away { get; set; }
        public string? message_kind_of_help { get; set; }
        public string? state { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
