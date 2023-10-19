using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    public abstract class EntityBase : IEntity
    {
        public abstract string? PrimaryKey { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
