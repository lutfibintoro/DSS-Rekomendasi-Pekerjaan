using BackEnd.Common;
using BackEnd.Domain;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Loker.Create;

public class CreateLokerHandler : IRequestHandler<CreateLokerCommand, GetAll.LokerDto>
{
    private readonly AppDbContext _db;
    public CreateLokerHandler(AppDbContext db) => _db = db;

    public async Task<GetAll.LokerDto> Handle(CreateLokerCommand cmd, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var perusahaan = new Perusahaan
        {
            NamaPerusahaan = cmd.NamaPerusahaan,
            PosisiTersedia = cmd.PosisiTersedia,
            Lokasi = cmd.Lokasi,
            CreatedAt = now
        };

        foreach (var n in cmd.NilaiPerKriteria)
        {
            perusahaan.Nilai.Add(new NilaiPerusahaan
            {
                KriteriaId = n.KriteriaId,
                NilaiRiil = n.NilaiRiil,
                NilaiSkala = NilaiKonverter.Konversi(n.KriteriaId, n.NilaiRiil),
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        _db.Perusahaan.Add(perusahaan);
        await _db.SaveChangesAsync(ct);

        return new GetAll.LokerDto(
            perusahaan.PerusahaanId, perusahaan.NamaPerusahaan,
            perusahaan.PosisiTersedia, perusahaan.Lokasi,
            perusahaan.Nilai.Select(n => new GetAll.NilaiLokerDto(
                n.NilaiId, n.PerusahaanId, n.KriteriaId, n.NilaiRiil, n.NilaiSkala)).ToList());
    }
}