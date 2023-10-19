using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [Table("HRDataSourceConnectionSettings", Schema = "dbo")]
    public class HRDataSourceConnectionSettings : EntityBase, IHRDataSourceConnectionSettings
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Desription { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? URL { get; set; }
        public string? TypeName { get; set; }
        public int? ExecutionOrder { get; set; }
        public override string? PrimaryKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
