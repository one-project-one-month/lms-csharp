namespace LearningManagementSystem.Domain.Models;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public bool IsError => Type == EnumResType.Error;
    public bool IsValidationError => Type == EnumResType.ValidationError;
    public bool IsSystemError => Type == EnumResType.SystemError;

    public EnumResType Type { get; private set; }
    public T? Data { get; private set; }
    public string Message { get; private set; } = null!;
    public List<string>? ValidationErrors { get; private set; }  // 🔹 Added for FluentValidation errors

    public static Result<T> Success(T data, string message = "Success")
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

    // 🔹 New: Method to handle multiple validation errors from FluentValidation
    public static Result<T> ValidationError(List<string> errors, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Type = EnumResType.ValidationError,
            Data = data,
            ValidationErrors = errors,
            Message = "Validation errors occurred."
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
