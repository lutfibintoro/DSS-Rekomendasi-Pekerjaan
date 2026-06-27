using BackEnd.Domain;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly AppDbContext _db;
    public RegisterHandler(AppDbContext db) => _db = db;

    public async Task<RegisterResult> Handle(RegisterCommand cmd, CancellationToken ct)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == cmd.Email, ct);
        if (exists)
            throw new InvalidOperationException("Email sudah terdaftar");

        var user = new User
        {
            Username = cmd.Username,
            Email = cmd.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(cmd.Password, workFactor: 11),
            Role = cmd.Role
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return new RegisterResult(user.UserId, user.Username, user.Email, user.Role);
    }
}