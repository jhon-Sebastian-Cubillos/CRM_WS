using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using Inmersys.Domain.DB.Schema.Profile;
using Inmersys.Domain.DB.Schema.Definition;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.Security
{
    public class Sec_Rol : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong rol_id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong profile_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("profile_id")]
        public virtual Pro_Profile? profile_info { get; set; }

        [ForeignKey("rol_id")]
        public virtual Def_Rol? rol_info { get; set; }
    }
}
