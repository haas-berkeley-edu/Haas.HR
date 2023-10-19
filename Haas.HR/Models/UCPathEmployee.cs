using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [Table("UCPathEmployee", Schema = "dbo")]
    public class UCPathEmployee : EmployeeBase
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }
}
