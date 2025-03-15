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

    public IActionResult Response<T>(Response<T> response)
    {
        var statusCode = response.Type switch
        {
            EnumResponseType.Success => 200,
            EnumResponseType.Error => 400,
            EnumResponseType.ValidationError => 400,
            EnumResponseType.SystemError => 500,
            _ => 500

        };

        // ApiDetails details = null;
        // object responseData = response.Data;


        var resp = new ApiResponse
        {
            Status = response.Type.ToString(),
            Data = response.Data,
            Error = response.IsSuccess ? null : response.Message,
            Message = response.IsSuccess ? response.Message : null,
            // Details = response.Data as ApiDetails ?? new ApiDetails()
        };

        return new ObjectResult(resp) { StatusCode = statusCode };
    }
}