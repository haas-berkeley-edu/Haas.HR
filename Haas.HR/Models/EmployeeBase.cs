using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [PrimaryKey(nameof(ID))]
    public class EmployeeBase : EntityBase, IEmployee
    {
        public string? ID { get; set; }

        public string? UID { get; set; }

        public override string? PrimaryKey
        {
            get { return this.ID; }
            set { }
        }
    }
}
