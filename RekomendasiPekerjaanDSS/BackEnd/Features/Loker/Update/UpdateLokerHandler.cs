using BackEnd.Common;
using BackEnd.Features.Loker.GetAll;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Loker.Update;

public class UpdateLokerHandler : IRequestHandler<UpdateLokerCommand, LokerDto>
{
    private readonly AppDbContext _db;
    public UpdateLokerHandler(AppDbContext db) => _db = db;

    public async Task<LokerDto> Handle(UpdateLokerCommand cmd, CancellationToken ct)
    {
        var p = await _db.Perusahaan
            .Include(x => x.Nilai)
            .FirstOrDefaultAsync(x => x.PerusahaanId == cmd.PerusahaanId, ct);
        if (p is null) throw new KeyNotFoundException("Loker tidak ditemukan");

        if (cmd.NamaPerusahaan is not null) p.NamaPerusahaan = cmd.NamaPerusahaan;
        if (cmd.PosisiTersedia is not null) p.PosisiTersedia = cmd.PosisiTersedia;
        if (cmd.Lokasi is not null) p.Lokasi = cmd.Lokasi;

        if (cmd.NilaiPerKriteria is not null)
        {
            // Replace strategy: hapus yang lama, insert yang baru
            _db.NilaiPerusahaan.RemoveRange(p.Nilai);
            var now = DateTime.UtcNow;
            foreach (var n in cmd.NilaiPerKriteria)
            {
                p.Nilai.Add(new Domain.NilaiPerusahaan
                {
                    PerusahaanId = p.PerusahaanId,
                    KriteriaId = n.KriteriaId,
                    NilaiRiil = n.NilaiRiil,
                    NilaiSkala = NilaiKonverter.Konversi(n.KriteriaId, n.NilaiRiil),
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
        }

        await _db.SaveChangesAsync(ct);

        return new LokerDto(
            p.PerusahaanId, p.NamaPerusahaan, p.PosisiTersedia, p.Lokasi,
            p.Nilai.Select(n => new NilaiLokerDto(
                n.NilaiId, n.PerusahaanId, n.KriteriaId, n.NilaiRiil, n.NilaiSkala)).ToList());
    }
}