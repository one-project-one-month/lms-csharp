using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Domain.Services.ResponseService;

public interface IResponseService
{
    IActionResult CreateResponse(int statusCode, string status, object data = null, string error = null,
        string message = null, ApiDetails details = null);

    IActionResult Response<T>(Response<T> response);
}

