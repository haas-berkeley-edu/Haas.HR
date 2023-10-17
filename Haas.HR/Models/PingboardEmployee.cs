using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [Table("PingboardEmployee", Schema = "dbo")]
    public class PingboardEmployee : EmployeeBase
    {
        public string Full_name { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Nickname { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string UID { get; set; }
        public DateTime Birthday { get; set; }
        public string Employee_Type { get; set; }
        public DateTime Updated_At { get; set; }
        public string Reports_to_email { get; set; }
        public string Reports_to_Name { get; set; }
        public string GitHub { get; set; }
        public string Avatar_Url { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string LastName { get; set; }
        public string Job_title { get; set; }
        public DateTime Start_date { get; set; }
        public string Interests { get; set; }
        public string Home_Address { get; set; }
        public string Linkedin { get; set; }
        public string Department { get; set; }
        public string Pingboard_UserID { get; set; }
        public string Skills { get; set; }
        public string Full_Job_title { get; set; }
        public string Groups { get; set; }
        public string Home_Email { get; set; }
        public string Home_Phone { get; set; }
        public string Office_phone { get; set; }
        public string Mobile { get; set; }
        public DateTime Last_Seen_At { get; set; }
        public DateTime Created_At { get; set; }

        [Column("T-Shirt_Size")]
        public string T_Shirt_Size { get; set; }
        public string Hear_my_name { get; set; }
        public string Emergency_Contact { get; set; }
        public string College { get; set; }
        public string Dietary_Restrictions { get; set; }
        public string Location { get; set; }
    }
}
