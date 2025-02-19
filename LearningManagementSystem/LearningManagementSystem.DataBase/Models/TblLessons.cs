using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblLessons
    {
        //public Guid id { get; set; } = Guid.NewGuid();
        //public Guid course_id { get; set; } = Guid.NewGuid();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int course_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string title { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string? videoUrl { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string lessonDetail { get; set; } = null!;

        [Required]
        [Column(TypeName = "bit")]
        public bool is_available { get; set; } = false;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }
        
        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;

        // 1 to M
        [Required]
        [ForeignKey("course_id")]
        public virtual TblCourses TblCourse { get; set; } = null!;


    }
}
