using System;
using System.Text.Json.Serialization;

namespace LearningManagementSystem.Domain.Services.UserServices.Responses;

public class UserResponse
{
    public int role_id { get; set; } // Validation Required for allow only student or instructor
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string phone { get; set; }
    public DateOnly dob { get; set; }
    public string address { get; set; }
    public string profile_photo { get; set; }
    public bool is_available { get; set; } = false;
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public string? role { get; set; }

    //Instructor
    public string nrc { get; set; }
    public string edu_background { get; set; }
    public int id { get; set; }
}
