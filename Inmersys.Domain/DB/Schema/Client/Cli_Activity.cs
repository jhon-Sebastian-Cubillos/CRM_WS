using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.Client
{
    public class Cli_Activity : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string name { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string desc { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong client_tab_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("client_tab_id")]
        public virtual Cli_Tab? client_tab_info { get; set; }

        public virtual ICollection<Cli_Asignment>? asignments_info { get; set; }
    }
}
