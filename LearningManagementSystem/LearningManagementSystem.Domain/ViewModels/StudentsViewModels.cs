namespace LearningManagementSystem.Domain.ViewModels;

public class StudentsViewModels
{
    public int role_id { get; set; } // Validation Required for allow only student or instructor
    public string username { get; set; } = null!;
    public string email { get; set; } = null!;
    public string password { get; set; } = null!;
    public string phone { get; set; } = null!;
    public DateOnly dob { get; set; }
    public string address { get; set; } = null!;
    public IFormFile profile_photo_file { get; set; } = null!;

    [JsonIgnore]
    public string profile_photo { get; set; } = null!;
    public bool is_available { get; set; } = false;
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    [JsonIgnore]
    public bool isDeleted { get; set; } = false;
}
