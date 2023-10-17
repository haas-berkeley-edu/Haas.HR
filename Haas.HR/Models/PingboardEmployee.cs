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
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
