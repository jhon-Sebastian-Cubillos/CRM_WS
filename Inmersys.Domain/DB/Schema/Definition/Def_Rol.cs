using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;
using Inmersys.Domain.DB.Schema.Security;

namespace Inmersys.Domain.DB.Schema.Definition
{
    public class Def_Rol : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string name { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string desc { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Sec_Rol>? security_profiles_info { get; set; }
        
        public virtual ICollection<Sec_Action>? security_actions_info { get; set; }

        public virtual ICollection<Sec_Window>? security_windows_info { get; set; }
    }
}
