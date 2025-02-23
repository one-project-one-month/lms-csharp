using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblCategory
    {
        //public Guid Category_Id { get; set; } = Guid.NewGuid();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string name { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;

        // 1 to M
        public virtual ICollection<TblCourses> TblCourses { get; set; } = new List<TblCourses>();
    }
}
