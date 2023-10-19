using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [Table("PingboardGroup", Schema = "dbo")]
    public class PingboardGroup : EntityBase
    {
        public string? id { get; set; }
        public string? created_at { get; set; }
        public string? updated_at { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public int memberships_count { get; set; }
        public string? description { get; set; }

        [NotMapped]
        public object? custom_fields { get; set; }
        public bool visible_to_owner { get; set; }
        public bool visible_to_group { get; set; }
        public bool visible_to_other { get; set; }
        public bool editable_by_owner { get; set; }
        public string? address { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }

        public override string? PrimaryKey { get => this.id; set => this.id = value; }
    }
}
