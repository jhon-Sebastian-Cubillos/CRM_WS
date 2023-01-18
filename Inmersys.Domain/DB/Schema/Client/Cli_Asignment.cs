using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;
using Inmersys.Domain.DB.Schema.Profile;

namespace Inmersys.Domain.DB.Schema.Client
{
    public class Cli_Asignment : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string name { get; set; }

        [Required, Column(TypeName = "nvarchar(255)")]
        public string desc { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong client_activity_id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong profile_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("profile_id")]
        public virtual Pro_Profile? profile_info { get; set; }

        [ForeignKey("client_activity_id")]
        public virtual Cli_Activity? activity_info { get; set; }
    }
}
