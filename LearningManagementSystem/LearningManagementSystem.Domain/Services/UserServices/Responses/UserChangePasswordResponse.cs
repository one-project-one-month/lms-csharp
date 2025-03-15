namespace LearningManagementSystem.Domain.Services.UserServices.Responses;

public class UserChangePasswordResponse
{
    public int id { get; set; }
    public string username { get; set; }
    public string newPassword { get; set; }
    public DateTime updated_at { get; set; }

}
