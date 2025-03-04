namespace LearningManagementSystem.Domain.Services.ResponseService;

public class ApiResponse
{
    public string Status { get; set; }
    public object Data { get; set; }
    public string Error { get; set; }
    public string Message { get; set; }
    public ApiDetails Details { get; set; }
}