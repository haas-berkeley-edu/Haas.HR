using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [Table("PingboardUser", Schema = "dbo")]
    public class PingboardUser : EntityBase
    {
        public string? id {  get; set; }
        public string? created_at { get; set; }
        public string? updated_at { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? nickname { get; set; }
        public string? start_date { get; set; }
        public string? time_zone { get; set; }
        public string? email { get; set; }
        public string? avatar_data { get; set; }
        public string? job_title { get; set; }
        public string? reports_to_id { get; set; }
        public string? bio { get; set; }
        public string? phone { get; set; }
        public string[]? skills { get; set; }
        public string[]? interests { get; set; }
        public string[]? roles { get; set; }
        public bool? email_message_channel { get; set; }
        public bool? phone_message_channel { get; set; }

        public string? UID { get; set; }

        [NotMapped]
        public object? custom_fields { get; set; }

        public override string? PrimaryKey { get => this.id; set => this.id = value; }
    }
}
