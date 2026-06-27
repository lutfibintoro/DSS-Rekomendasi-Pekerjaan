namespace BackEnd.Common;

public record ApiResponse<T>(bool Success, string Message, T? Data, object? Errors = null)
{
    public static ApiResponse<T> Ok(T data, string message = "OK") => new(true, message, data);
    public static ApiResponse<T> Fail(string message, object? errors = null) => new(false, message, default, errors);
}