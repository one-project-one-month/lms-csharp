namespace LearningManagementSystem.Domain.Services.UserServices.Requests;

public class UserChangePasswordRequest
{
    public int id { get; set; }
    public string OldPassword { get; set; }
    public string newPassword { get; set; }
    public string confirmPassword { get; set; }
    public DateTime created_at { get; set; }
}
