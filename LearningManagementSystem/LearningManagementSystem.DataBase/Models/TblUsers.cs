using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.DataBase.Models;

public class TblUsers
{
    //public Guid id { get; set; } = Guid.NewGuid();

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }

    [Required]
    //[RegularExpression("Student|Instructor")]
    public int role_id { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string username { get; set; } = null!;

    [EmailAddress]
    [Column(TypeName = "varchar(255)")]
    public string email { get; set; } = null!;

    
    [Column(TypeName = "varchar(255)")]
    public string password { get; set; } = null!;

    [Length(13, 13)]
    [Column(TypeName = "varchar(255)")]
    public string phone { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateOnly dob { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string address { get; set; } = null!;

    [Column(TypeName = "varchar(255)")]
    public string profile_photo { get; set; } = null!;

    [Column(TypeName = "bit")]
    public bool is_available { get; set; } = false;

    [Column(TypeName = "datetime")]
    public DateTime created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? updated_at { get; set; }

    [Column(TypeName = "bit")]
    public bool isDeleted { get; set; } = false;

    // 1 to M
    [Required]
    [ForeignKey("role_id")]
    public virtual TblRoles TblRole { get; set; } = null!;
    // 1 to 1
    public virtual TblStudents? TblStudent { get; set; }
    // 1 to 1
    public virtual TblAdmins? TblAdmin { get; set; }
    // 1 to 1
    public virtual TblInstructors? TblInstructor { get; set; }
    // M to M
    public virtual ICollection<TblEnrollments> TblEnrollments { get; set; } = new List<TblEnrollments>();

    // 1 to 1
    public virtual TblTokens Tokens { get; set; } = null!;

}
