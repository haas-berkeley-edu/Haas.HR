using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    public class HrDataSourceConection : EntityBase
    {
        public override string? PrimaryKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int? ID { get; set; }
        string? Name { get; set; }
        string? Desription { get; set; }
        string? UserName { get; set; }
        string? Password { get; set; }
        string? URL { get; set; }
        string? Type { get; set; }
        int? ExecutionOrder { get; set; }
        
    }
}
