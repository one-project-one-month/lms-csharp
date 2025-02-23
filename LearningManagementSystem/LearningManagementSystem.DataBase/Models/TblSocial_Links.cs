using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.DataBase.Models
{
    public class TblSocial_Links
    {
        //public Guid id { get; set; } = Guid.NewGuid();
        //public Guid course_id { get; set; } = Guid.NewGuid();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int course_id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? facebook { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? X { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? telegram { get; set; }

        [Length(13, 13)]
        [Column(TypeName = "varchar(255)")]
        public string? phone { get; set; }

        [EmailAddress]
        [Column(TypeName = "varchar(255)")]
        public string? email { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;
        //1 to 1
        [Required]
        [ForeignKey("course_id")]
        public virtual TblCourses TblCourse { get; set; } = null!;
    }
}
