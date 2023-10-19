using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    public interface IEmployee : IEntity
    {
        string? ID { get; set; }

        string? UID { get; set; }
    }
}
