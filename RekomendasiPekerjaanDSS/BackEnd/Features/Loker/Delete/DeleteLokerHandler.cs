using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Loker.Delete;

public class DeleteLokerHandler : IRequestHandler<DeleteLokerCommand, DeleteLokerResult>
{
    private readonly AppDbContext _db;
    public DeleteLokerHandler(AppDbContext db) => _db = db;

    public async Task<DeleteLokerResult> Handle(DeleteLokerCommand cmd, CancellationToken ct)
    {
        var p = await _db.Perusahaan.FirstOrDefaultAsync(x => x.PerusahaanId == cmd.PerusahaanId, ct);
        if (p is null) throw new KeyNotFoundException("Loker tidak ditemukan");

        _db.Perusahaan.Remove(p);
        await _db.SaveChangesAsync(ct);
        return new DeleteLokerResult(p.PerusahaanId);
    }
}