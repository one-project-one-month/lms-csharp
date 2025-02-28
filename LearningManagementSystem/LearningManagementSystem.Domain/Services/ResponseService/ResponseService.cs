using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Domain.Services.ResponseService;

public class ResponseService : IResponseService
{
    public IActionResult CreateResponse(int statusCode, string status, object data = null, string error = null,
        string message = null, ApiDetails details = null)
    {
        var response = new ApiResponse
        {
            Status = status,
            Data = data,
            Error = error,
            Message = message,
            Details = details
        };

        return new ObjectResult(response) { StatusCode = statusCode };
    }
}