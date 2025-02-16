using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataBase.Models.Users;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    //public Guid id { get; set; } = Guid.NewGuid();
    public string username { get; set; } = null!;
    [EmailAddress]
    public string email { get; set; } = null!;

    [MinLength(6)]
    [MaxLength(16)]
    public string password { get; set; } = null!;
    public string phone { get; set; } = null!;
    public DateOnly dob { get; set; }
    public string address { get; set; } = null!;
    public string profile_photo { get; set; } = null!;
    [Required]
    [RegularExpression("Student|Instructor")]
    public string role_id { get; set; } = null!;
    public bool is_available { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public bool DeleteFlag { get; set; } = false;
}
