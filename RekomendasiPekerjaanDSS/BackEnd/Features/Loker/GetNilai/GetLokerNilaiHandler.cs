using BackEnd.Features.Loker.GetAll;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Loker.GetNilai;

public class GetLokerNilaiHandler : IRequestHandler<GetLokerNilaiQuery, List<NilaiLokerDto>>
{
    private readonly AppDbContext _db;
    public GetLokerNilaiHandler(AppDbContext db) => _db = db;

    public async Task<List<NilaiLokerDto>> Handle(GetLokerNilaiQuery req, CancellationToken ct)
    {
        var exists = await _db.Perusahaan.AsNoTracking()
            .AnyAsync(p => p.PerusahaanId == req.PerusahaanId, ct);
        if (!exists) throw new KeyNotFoundException("Loker tidak ditemukan");

        var list = await _db.NilaiPerusahaan.AsNoTracking()
            .Where(n => n.PerusahaanId == req.PerusahaanId)
            .OrderBy(n => n.KriteriaId)
            .ToListAsync(ct);

        return list.Select(n => new NilaiLokerDto(
            n.NilaiId, n.PerusahaanId, n.KriteriaId, n.NilaiRiil, n.NilaiSkala)).ToList();
    }
}