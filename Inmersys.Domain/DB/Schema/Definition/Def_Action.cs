using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;
using Inmersys.Domain.DB.Schema.Security;

namespace Inmersys.Domain.DB.Schema.Definition
{
    public class Def_Action : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string controller_name { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string method_name { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        public ICollection<Sec_Action>? security_roles_info { get; set; }
    }
}
