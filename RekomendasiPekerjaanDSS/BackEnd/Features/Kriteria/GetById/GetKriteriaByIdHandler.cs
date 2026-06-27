using BackEnd.Features.Kriteria.GetAll;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Kriteria.GetById;

public class GetKriteriaByIdHandler : IRequestHandler<GetKriteriaByIdQuery, KriteriaDto>
{
    private readonly AppDbContext _db;
    public GetKriteriaByIdHandler(AppDbContext db) => _db = db;

    public async Task<KriteriaDto> Handle(GetKriteriaByIdQuery req, CancellationToken ct)
    {
        var k = await _db.Kriteria.AsNoTracking()
            .FirstOrDefaultAsync(x => x.KriteriaId == req.KriteriaId, ct);
        if (k is null) throw new KeyNotFoundException("Kriteria tidak ditemukan");

        return new KriteriaDto(
            k.KriteriaId, k.NamaKriteria, k.JenisAtribut,
            k.Satuan, k.Deskripsi,
            SkalaKriteriaMap.Get(k.KriteriaId));
    }
}