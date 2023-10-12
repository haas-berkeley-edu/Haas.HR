using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    public interface IEmployee
    {
        string ID { get; set; }

        string UID { get; set; }

        DateTime? CreateOn { get; set; }
        DateTime? LastUpdatedOn { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
