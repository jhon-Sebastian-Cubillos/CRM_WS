using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.Client
{
    public class Cli_Tab : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string name { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string desc { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong client_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("client_id")]
        public virtual Cli_Info? client_info { get; set; }
        
        public virtual ICollection<Cli_Activity>? tab_activities_info { get; set; }
    }
}
