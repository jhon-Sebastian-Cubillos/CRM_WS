using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inmersys.Domain.DB.Base;
using System.Text.Json.Serialization;

namespace Inmersys.Domain.DB.Schema.History
{
    public class His_Changes : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(TypeName = "bigint")]
        public ulong id { get; set; }

        [Required, Column(TypeName = "varchar(80)")]
        public string action { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string preview_change { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string change { get; set; }

        [Required, Column(TypeName = "bigint")]
        public ulong transact_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime registred_date { get; set; } = DateTime.UtcNow;

        [ForeignKey("transact_id")]
        public virtual His_Transaction? transaction_info { get; set; }
    }
}
