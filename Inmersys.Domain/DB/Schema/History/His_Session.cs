using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.History
{
    public class His_Session : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string Jti { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string req_ip { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string client_loc { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong user_id { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong rol_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime exp_date { get; set; }

        public virtual ICollection<His_Transaction>? transactions_info { get; set; }
    }
}
