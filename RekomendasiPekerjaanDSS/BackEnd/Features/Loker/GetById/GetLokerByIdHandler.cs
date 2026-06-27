using BackEnd.Features.Loker.GetAll;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Loker.GetById;

public class GetLokerByIdHandler : IRequestHandler<GetLokerByIdQuery, LokerDto>
{
    private readonly AppDbContext _db;
    public GetLokerByIdHandler(AppDbContext db) => _db = db;

    public async Task<LokerDto> Handle(GetLokerByIdQuery req, CancellationToken ct)
    {
        var p = await _db.Perusahaan.AsNoTracking()
            .Include(x => x.Nilai)
            .FirstOrDefaultAsync(x => x.PerusahaanId == req.PerusahaanId, ct);

        if (p is null) throw new KeyNotFoundException("Loker tidak ditemukan");

        return new LokerDto(
            p.PerusahaanId, p.NamaPerusahaan, p.PosisiTersedia, p.Lokasi,
            p.Nilai.Select(n => new NilaiLokerDto(
                n.NilaiId, n.PerusahaanId, n.KriteriaId, n.NilaiRiil, n.NilaiSkala)).ToList());
    }
}