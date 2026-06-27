using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Kriteria.GetAll;

public record KriteriaDto(
    string KriteriaId,
    string NamaKriteria,
    string JenisAtribut,
    string? Satuan,
    string? Deskripsi,
    Dictionary<string, string> Skala
);

public record GetAllKriteriaQuery() : IRequest<List<KriteriaDto>>;

public class GetAllKriteriaHandler : IRequestHandler<GetAllKriteriaQuery, List<KriteriaDto>>
{
    private readonly AppDbContext _db;
    public GetAllKriteriaHandler(AppDbContext db) => _db = db;

    public async Task<List<KriteriaDto>> Handle(GetAllKriteriaQuery req, CancellationToken ct)
    {
        var list = await _db.Kriteria.AsNoTracking()
            .OrderBy(k => k.KriteriaId)
            .ToListAsync(ct);

        return list.Select(k => new KriteriaDto(
            k.KriteriaId,
            k.NamaKriteria,
            k.JenisAtribut,
            k.Satuan,
            k.Deskripsi,
            SkalaKriteriaMap.Get(k.KriteriaId)
        )).ToList();
    }
}