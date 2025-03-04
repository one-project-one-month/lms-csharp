using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblTokens
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string token { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;

        //[Required]
        [ForeignKey("user_id")]
        [JsonIgnore]
        public virtual TblUsers? TblUser { get; set; } 
    }
}
