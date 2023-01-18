using Inmersys.Domain.DB.Base;
using Inmersys.Domain.DB.Schema.Client;
using Inmersys.Domain.DB.Schema.Definition;
using Inmersys.Domain.DB.Schema.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.Profile
{
    public class Pro_Profile : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string f_name { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string l_name { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong user_id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string login { get; set; }

        [Required, Column(TypeName = "varchar(80)"), EmailAddress]
        public string email { get; set; }

        [Required]
        public byte[] p_key { get; set; }
        
        [Required]
        public byte[] password { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime created_date { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "smallint")]
        public byte tries { get; set; } = 0;

        public bool blocked { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime last_log { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "bigint")]
        public ulong gender_id { get; set; }

        public virtual Def_Gender? gender_info { get; set; }

        public virtual ICollection<Sec_Rol>? security_roles_info { get; set; }
        
        public virtual ICollection<Cli_Asignment>? client_asignments_info { get; set; }
    }
}
