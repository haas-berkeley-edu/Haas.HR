using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    public class EmployeeBase : IEmployee
    {
        public string ID { get; set; }

        public string UID { get; set; }

        public DateTime? CreateOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
