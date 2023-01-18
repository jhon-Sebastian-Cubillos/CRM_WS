using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.History
{
    public class His_Transaction : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }
        
        [Required, Column(TypeName = "nvarchar(max)")]
        public string token_desc { get; set; }

        [Required, Column(TypeName = "datetime")]
        public DateTime transaction_date { get; set; } = DateTime.UtcNow;

        [Required, Column(TypeName = "bigint")]
        public ulong session_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("session_id")]
        public virtual His_Session? session_info { get; set; }

        public virtual ICollection<His_Changes>? changes_info { get; set; }
    }
}
