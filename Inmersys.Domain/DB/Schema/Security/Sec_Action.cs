using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using Inmersys.Domain.DB.Schema.Definition;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.Security
{
    public class Sec_Action : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong rol_id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong action_id { get; set; }

        [Required]
        public bool authorized { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("action_id")]
        public virtual Def_Action? action_info { get; set; }

        [ForeignKey("rol_id")]
        public virtual Def_Rol? rol_info { get; set; }
    }
}
