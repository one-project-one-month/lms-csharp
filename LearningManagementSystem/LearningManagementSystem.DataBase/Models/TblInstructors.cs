using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblInstructors
    {
        //public Guid id { get; set; } = Guid.NewGuid();
        //public Guid user_id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int user_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string nrc { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string edu_background { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;

        // 1 to 1
        [Required]
        [ForeignKey("user_id")]
        public virtual TblUsers TblUser { get; set; } = null!;

        //1 to M
        public virtual ICollection<TblCourses> TblCourses { get; set; } = new List<TblCourses>();
    }
}
