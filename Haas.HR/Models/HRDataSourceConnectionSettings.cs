using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    public class HRDataSourceConnectionSettings : IHRDataSourceConnectionSettings
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Desription { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }
        public string TypeName { get; set; }
        public int ExecutionOrder { get; set; }
    }
}
