using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Preferences.Reset;

public class ResetPreferencesHandler : IRequestHandler<ResetPreferencesCommand, ResetPreferencesResult>
{
    private readonly AppDbContext _db;
    public ResetPreferencesHandler(AppDbContext db) => _db = db;

    public async Task<ResetPreferencesResult> Handle(ResetPreferencesCommand cmd, CancellationToken ct)
    {
        var rows = await _db.PreferensiUser.Where(p => p.UserId == cmd.UserId).ToListAsync(ct);
        _db.PreferensiUser.RemoveRange(rows);
        await _db.SaveChangesAsync(ct);
        return new ResetPreferencesResult(rows.Count);
    }
}