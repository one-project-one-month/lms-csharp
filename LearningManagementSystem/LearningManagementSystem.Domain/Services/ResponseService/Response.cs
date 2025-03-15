namespace LearningManagementSystem.Domain.Services.ResponseService;

public class Response<T>
{
    public EnumResponseType Type { get; private set; }
    public T? Data { get; private set; }
    public string Message { get; private set; } = null!;


    public bool IsSuccess { get; set; }
    public bool IsError { get { return Type == EnumResponseType.Error; } }
    public bool IsValidationError { get { return Type == EnumResponseType.ValidationError; } }
    public bool IsSystemError { get { return Type == EnumResponseType.SystemError; } }


    public static Response<T> Success<T>(T data, string message = "Success")
    {
        return new Response<T>
        {
            IsSuccess = true,
            Type = EnumResponseType.Success,
            Data = data,
            Message = message
        };
    }

    public static Response<T> Error(string message, T? data = default)
    {
        return new Response<T>
        {
            IsSuccess = false,
            Type = EnumResponseType.Error,
            Data = data,
            Message = message
        };
    }

    public static Response<T> ValidationError(string message, T? data = default)
    {
        return new Response<T>
        {
            IsSuccess = false,
            Type = EnumResponseType.ValidationError,
            Data = data,
            Message = message
        };
    }

    public static Response<T> SystemError(string message, T? data = default)
    {
        return new Response<T>
        {
            IsSuccess = false,
            Type = EnumResponseType.SystemError,
            Data = data,
            Message = message
        };
    }


}

public enum EnumResponseType
{
    None,
    Success,
    Error,
    ValidationError,
    SystemError
}
