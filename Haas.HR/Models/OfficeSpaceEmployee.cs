using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Haas.HR.Models
{
    [Table("OfficeSpaceEmployee", Schema = "dbo")]
    public class OfficeSpaceEmployee : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id {  get; set; }

        public string? client_employee_id { get; set; }
        public string? department { get; set; }
        public string? email { get; set; }
        public string? end_date { get; set; }
        public string? extension { get; set; }
        public string? first_name { get; set; }
        public string? initials { get; set; }
        public string? image_fingerprint { get; set; }
        public string? image_source_fingerprint { get; set; }
        public string? last_name { get; set; }
        public string? location { get; set; }
        public string? notes { get; set; }

        [NotMapped]
        public object? online_seating { get; set; }
        public string? photo { get; set; }

        [NotMapped] 
        public object? seating { get; set; }

        [NotMapped] 
        public object? show_in_vd { get; set; }
        public string? source { get; set; }
        public string? start_date { get; set; }
        public string? title { get; set; }
        public string? udf0 { get; set; }
        public string? udf10 { get; set; }
        public string? udf11 { get; set; }
        public string? udf12 { get; set; }
        public string? udf13 { get; set; }
        public string? udf14 { get; set; }
        public string? udf15 { get; set; }
        public string? udf16 { get; set; }
        public string? udf17 { get; set; }
        public string? udf18 { get; set; }
        public string? udf19 { get; set; }
        public string? udf1 { get; set; }
        public string? udf20 { get; set; }
        public string? udf21 { get; set; }
        public string? udf22 { get; set; }
        public string? udf23 { get; set; }
        public string? udf24 { get; set; }
        public string? udf2 { get; set; }
        public string? udf3 { get; set; }
        public string? udf4 { get; set; }
        public string? udf5 { get; set; }
        public string? udf6 { get; set; }
        public string? udf7 { get; set; }
        public string? udf8 { get; set; }
        public string? udf9 { get; set; }
        public string? work_phone { get; set; }

        public override string? PrimaryKey {
            get
            {
                if (this.id != null)
                {
                    return this.id.ToString();
                }
                return null;
            }
            set { }
        }
    }
}
