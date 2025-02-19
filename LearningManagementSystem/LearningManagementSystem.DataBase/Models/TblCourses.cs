using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblCourses
    {
        //public Guid id { get; set; } = Guid.NewGuid();
        //public Guid instructors_id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int instructor_id { get; set; }
        public int category_id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string courseName { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string thumbnail { get; set; } = null!;

        [Required]
        [Column(TypeName = "bit")]
        public bool is_available { get; set; } = false;

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string type { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string level { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string description { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime duration { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? original_price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal current_price { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;

        // 1 to 1
        public virtual TblSocial_Links? TblSocial_Link { get; set; }

        // 1 to M
        public virtual ICollection<TblLessons> TblLessons { get; set; } = new List<TblLessons>();

        // 1 to M
        [Required]
        [ForeignKey("instructor_id")]
        public virtual TblInstructors TblInstructor { get; set; } = null!;

        // 1 to M
        [Required]
        [ForeignKey("category_id")]
        public virtual TblCategory TblCategory { get; set; } = null!;

        // M to M
        public virtual ICollection<TblEnrollments> TblEnrollments { get; set; } = new List<TblEnrollments>();
    }
}
