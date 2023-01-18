using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;
using Inmersys.Domain.DB.Schema.Definition;

namespace Inmersys.Domain.DB.Schema.Security
{
    public class Sec_Window : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong window_id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong rol_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("rol_id")]
        public virtual Def_Rol? rol_info { get; set; }

        [ForeignKey("window_id")]
        public virtual Def_Window? window_info { get; set; }
    }
}
