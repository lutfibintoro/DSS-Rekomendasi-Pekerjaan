using BackEnd.Common;
using BackEnd.Domain;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Preferences.Upsert;

public class UpsertPreferencesHandler : IRequestHandler<UpsertPreferencesCommand, List<PreferensiDto>>
{
    private readonly AppDbContext _db;
    public UpsertPreferencesHandler(AppDbContext db) => _db = db;

    public async Task<List<PreferensiDto>> Handle(UpsertPreferencesCommand cmd, CancellationToken ct)
    {
        // Validasi semua kriteria_id ada di master
        var ids = cmd.Preferences.Select(p => p.KriteriaId).Distinct().ToList();
        var validIds = await _db.Kriteria
            .Where(k => ids.Contains(k.KriteriaId))
            .Select(k => k.KriteriaId)
            .ToListAsync(ct);
        var invalid = ids.Except(validIds).ToList();
        if (invalid.Count > 0)
            throw new InvalidOperationException($"Kriteria tidak valid: {string.Join(", ", invalid)}");

        // Hapus preferensi lama user ini
        var existing = await _db.PreferensiUser
            .Where(p => p.UserId == cmd.UserId)
            .ToListAsync(ct);
        _db.PreferensiUser.RemoveRange(existing);

        // Insert yang baru dengan konversi nilai_asli -> nilai_bobot
        var now = DateTime.UtcNow;
        var newEntities = cmd.Preferences.Select(p =>
        {
            var bobot = NilaiKonverter.Konversi(p.KriteriaId, p.NilaiAsli);
            return new PreferensiUser
            {
                UserId = cmd.UserId,
                KriteriaId = p.KriteriaId,
                NilaiAsli = p.NilaiAsli,
                NilaiBobot = bobot,
                CreatedAt = now,
                UpdatedAt = now
            };
        }).ToList();

        _db.PreferensiUser.AddRange(newEntities);
        await _db.SaveChangesAsync(ct);

        return newEntities.Select(p => new PreferensiDto(
            p.PreferenceId, p.UserId, p.KriteriaId, p.NilaiAsli, p.NilaiBobot)).ToList();
    }
}