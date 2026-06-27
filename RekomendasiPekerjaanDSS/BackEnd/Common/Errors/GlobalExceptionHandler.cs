using BackEnd.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace BackEnd.Common.Errors;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
    {
        logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

        var (status, message, errors) = ex switch
        {
            AppValidationException ave => (400, "Validasi gagal", ave.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),
            KeyNotFoundException => (404, "Data tidak ditemukan", null),
            UnauthorizedAccessException => (403, "Akses ditolak", null),
            InvalidOperationException => (400, ex.Message, null),
            _ => (500, "Terjadi kesalahan internal", null)
        };

        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        var response = ApiResponse<object>.Fail(message, errors);
        await ctx.Response.WriteAsJsonAsync(response, ct);
        return true;
    }
}

public class AppValidationException : Exception
{
    public IReadOnlyList<FluentValidation.Results.ValidationFailure> Errors { get; }
    public AppValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> errors)
        : base("Validasi gagal") => Errors = errors.ToList();
}