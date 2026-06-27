using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly AppDbContext _db;
    public LoginHandler(AppDbContext db) => _db = db;

    public async Task<LoginResult> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == cmd.Email, ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(cmd.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email atau password salah");

        return new LoginResult(user.UserId, user.Username, user.Email, user.Role);
    }
}