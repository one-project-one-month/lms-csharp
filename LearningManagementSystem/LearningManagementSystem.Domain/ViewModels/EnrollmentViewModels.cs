using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Domain.ViewModels;

public class EnrollmentViewModels
{
    //public Guid id { get; set; } = Guid.Empty!;

    [JsonIgnore]
    public int id { get; set; }

    public int user_id { get; set; }

    public int course_id { get; set; }

    public DateTime enrollment_date { get; set; }

    [Required]
    public bool is_completed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? completed_date { get; set; }

    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    [JsonIgnore]
    public bool isDeleted { get; set; } = false;

}