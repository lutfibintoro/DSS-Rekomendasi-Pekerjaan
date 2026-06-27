using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Loker.GetAll;

public class GetAllLokerHandler : IRequestHandler<GetAllLokerQuery, List<LokerDto>>
{
    private readonly AppDbContext _db;
    public GetAllLokerHandler(AppDbContext db) => _db = db;

    public async Task<List<LokerDto>> Handle(GetAllLokerQuery req, CancellationToken ct)
    {
        var companies = await _db.Perusahaan.AsNoTracking()
            .OrderByDescending(p => p.PerusahaanId)
            .Include(p => p.Nilai)
            .ToListAsync(ct);

        return companies.Select(p => new LokerDto(
            p.PerusahaanId,
            p.NamaPerusahaan,
            p.PosisiTersedia,
            p.Lokasi,
            p.Nilai.Select(n => new NilaiLokerDto(
                n.NilaiId, n.PerusahaanId, n.KriteriaId, n.NilaiRiil, n.NilaiSkala)).ToList()
        )).ToList();
    }
}