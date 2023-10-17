using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [Table("SupervisorCalGroupEmployee", Schema = "dbo")]
    public class SupervisorCalGroupEmployee : EmployeeBase
    {
    }
}
