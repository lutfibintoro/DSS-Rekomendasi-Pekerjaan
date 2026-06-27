namespace BackEnd.Domain;

public class NilaiPerusahaan
{
    public int NilaiId { get; set; }
    public int PerusahaanId { get; set; }
    public string KriteriaId { get; set; } = string.Empty;
    public string NilaiRiil { get; set; } = string.Empty;
    public short NilaiSkala { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Perusahaan? Perusahaan { get; set; }
    public Kriteria? Kriteria { get; set; }
}