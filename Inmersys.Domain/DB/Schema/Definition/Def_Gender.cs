using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using Inmersys.Domain.DB.Schema.Profile;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.Definition
{
    public class Def_Gender : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string name { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string desc { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public ICollection<Pro_Profile>? profiles_info { get; set; }
    }
}
