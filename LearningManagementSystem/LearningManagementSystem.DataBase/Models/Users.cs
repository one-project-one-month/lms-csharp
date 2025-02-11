using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataBase.Models;

public class Users
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    [MinLength(6)]
    [MaxLength(16)]
    public string PasswordHash { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    [Required]
    [RegularExpression("Student|Instructor")]
    public string Role { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public bool DeleteFlag { get; set; } = false;
}
