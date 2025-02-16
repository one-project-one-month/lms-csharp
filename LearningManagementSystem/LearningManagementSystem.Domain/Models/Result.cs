namespace LearningManagementSystem.Domain.Models;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public bool IsError { get { return Type == EnumResType.Error; } }
    public bool IsValidationError { get { return Type == EnumResType.ValidationError; } }
    public bool IsSystemError { get { return Type == EnumResType.SystemError; } }

    public EnumResType Type { get; private set; }
    public T? Data { get; private set; }
    public string Message { get; private set; } = null!;

    public static Result<T> Success<T>(T data, string message = "Success")
    {
        return new Result<T>
        {
            IsSuccess = true,
            Type = EnumResType.Success,
            Data = data,
            Message = message

        };
    }

    public static Result<T> ValidationError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Type = EnumResType.ValidationError,
            Data = data,
            Message = message
        };
    }

    public static Result<T> SystemError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Type = EnumResType.SystemError,
            Data = data,
            Message = message
        };
    }
    public static Result<T> Error(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Type = EnumResType.Error,
            Data = data,
            Message = message
        };
    }

}

public enum EnumResType
{
    None,
    Success,
    Error,
    ValidationError,
    SystemError
}
