namespace BackEnd.Features.Rekomendasi;

public record NilaiDetailDto(string KriteriaId, short NilaiSkala, string NilaiRiil);

public record RekomendasiItem(
    int Rank,
    int PerusahaanId,
    string NamaPerusahaan,
    string? PosisiTersedia,
    string? Lokasi,
    double SkorSaw,
    List<NilaiDetailDto> DetailNilai
);