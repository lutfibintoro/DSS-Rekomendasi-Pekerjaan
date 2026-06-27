using System.Security.Claims;
using BackEnd.Domain;
using BackEnd.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Common.Auth;

/// <summary>
/// Helper untuk mengambil info user yang sedang login dari HttpContext.
/// Di-set oleh BasicAuthHandler setelah kredensial tervalidasi.
/// Juga menyediakan helper cek role untuk endpoint.
/// </summary>
public class CurrentUser
{
    private readonly IHttpContextAccessor _accessor;
    private readonly AppDbContext? _db;

    public CurrentUser(IHttpContextAccessor accessor, IServiceProvider sp)
    {
        _accessor = accessor;
        var http = accessor.HttpContext;
        if (http?.Items.TryGetValue("CurrentUser", out var raw) == true && raw is User u)
        {
            User = u;
        }
        else if (http?.User?.Identity?.IsAuthenticated == true)
        {
            var userIdStr = http.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out var uid))
            {
                _db = sp.GetService(typeof(AppDbContext)) as AppDbContext;
                User = _db?.Users.AsNoTracking().FirstOrDefault(x => x.UserId == uid);
                if (User is not null) http.Items["CurrentUser"] = User;
            }
        }
    }

    public User? User { get; }

    public bool IsAuthenticated => User is not null;
    public int UserId => User?.UserId ?? 0;
    public string Email => User?.Email ?? "";
    public string Username => User?.Username ?? "";
    public string Role => User?.Role ?? "";
    public bool IsAdmin => User?.Role == "admin";
    public bool IsPelamar => User?.Role == "pelamar";
}