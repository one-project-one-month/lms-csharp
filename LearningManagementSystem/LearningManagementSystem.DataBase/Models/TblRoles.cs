using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblRoles
    {
        //public Guid id { get; set; } = Guid.NewGuid();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string role { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;
        //1 to M
        public virtual ICollection<TblUsers> TblUsers { get; set; } = new List<TblUsers>();
    }
}
