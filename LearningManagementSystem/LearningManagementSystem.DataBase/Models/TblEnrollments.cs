using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblEnrollments
    {
        //public Guid id { get; set; } = Guid.NewGuid();
        //public Guid user_id { get; set; } = Guid.NewGuid();
        //public Guid course_id { get; set; } = Guid.NewGuid();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int user_id { get; set; }
        public int course_id { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime enrollment_date { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool is_completed { get; set; } = false;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? completed_date { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;

        // 1 to M
        [Required]
        [ForeignKey("user_id")]
        //[ForeignKey(nameof(TblUser))]
        public virtual TblUsers TblUser { get; set; } = null!;

        // 1 to M
        [Required]
        [ForeignKey("course_id")]
        //[ForeignKey(nameof(TblCourse))]
        public virtual TblCourses TblCourse { get; set; } = null!;

    }
}
