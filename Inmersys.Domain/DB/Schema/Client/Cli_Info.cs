using Inmersys.Domain.DB.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.Client
{
    public class Cli_Info : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string name { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong nit { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime created_date { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Cli_Tab>? cient_tabs_info { get; set; }
    }
}
