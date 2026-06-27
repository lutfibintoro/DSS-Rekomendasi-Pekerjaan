using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Rekomendasi;

public class GetRekomendasiHandler : IRequestHandler<GetRekomendasiQuery, List<RekomendasiItem>>
{
    private readonly AppDbContext _db;
    public GetRekomendasiHandler(AppDbContext db) => _db = db;

    public async Task<List<RekomendasiItem>> Handle(GetRekomendasiQuery req, CancellationToken ct)
    {
        // 1. Bobot preferensi user (w_j)
        var prefs = await _db.PreferensiUser.AsNoTracking()
            .Where(p => p.UserId == req.UserId)
            .ToListAsync(ct);
        if (prefs.Count == 0)
            throw new KeyNotFoundException("User belum memiliki preferensi");

        var weights = prefs.ToDictionary(p => p.KriteriaId, p => (double)p.NilaiBobot);

        // 2. Master kriteria untuk jenis_atribut (benefit/cost)
        var kriterias = await _db.Kriteria.AsNoTracking()
            .ToDictionaryAsync(k => k.KriteriaId, k => k.JenisAtribut, ct);

        // 3. Semua loker + nilai
        var lokers = await _db.Perusahaan.AsNoTracking()
            .Include(p => p.Nilai)
            .ToListAsync(ct);

        if (lokers.Count == 0) return [];

        // 4. Bangun matriks keputusan: loker_id -> kriteria_id -> nilai_skala
        var matrix = new Dictionary<int, Dictionary<string, double>>();
        var detailMap = new Dictionary<int, List<NilaiDetailDto>>();
        foreach (var p in lokers)
        {
            var row = new Dictionary<string, double>();
            var detail = new List<NilaiDetailDto>();
            foreach (var n in p.Nilai)
            {
                if (weights.ContainsKey(n.KriteriaId))
                {
                    row[n.KriteriaId] = n.NilaiSkala;
                    detail.Add(new NilaiDetailDto(n.KriteriaId, n.NilaiSkala, n.NilaiRiil));
                }
            }
            matrix[p.PerusahaanId] = row;
            detailMap[p.PerusahaanId] = detail;
        }

        // 5. Normalisasi SAW per kriteria
        var normalized = new Dictionary<int, Dictionary<string, double>>();
        foreach (var kId in weights.Keys)
        {
            var colValues = matrix.Values
                .Where(r => r.ContainsKey(kId))
                .Select(r => r[kId])
                .ToList();
            if (colValues.Count == 0) continue;

            var isBenefit = kriterias.TryGetValue(kId, out var jenis) && jenis == "benefit";
            double max = colValues.Max();
            double min = colValues.Min();

            foreach (var (perusahaanId, row) in matrix)
            {
                if (!row.TryGetValue(kId, out var v)) continue;
                double r = isBenefit ? v / max : min / v;
                if (!normalized.ContainsKey(perusahaanId))
                    normalized[perusahaanId] = new Dictionary<string, double>();
                normalized[perusahaanId][kId] = r;
            }
        }

        // 6. Skor SAW per alternatif: V_i = Σ (r_ij × w_j)
        var scores = matrix.Keys.Select(perusahaanId =>
        {
            double v = 0;
            if (normalized.TryGetValue(perusahaanId, out var row))
            {
                foreach (var (kId, w) in weights)
                {
                    if (row.TryGetValue(kId, out var r))
                        v += r * w;
                }
            }
            return (perusahaanId, score: v);
        }).OrderByDescending(x => x.score)
          .ToList();

        // 7. Map ke DTO dengan rank
        var companyMap = lokers.ToDictionary(p => p.PerusahaanId);
        var result = scores.Select((s, idx) =>
        {
            var p = companyMap[s.perusahaanId];
            return new RekomendasiItem(
                idx + 1,
                p.PerusahaanId,
                p.NamaPerusahaan,
                p.PosisiTersedia,
                p.Lokasi,
                Math.Round(s.score, 4),
                detailMap[s.perusahaanId]
            );
        }).ToList();

        return result;
    }
}