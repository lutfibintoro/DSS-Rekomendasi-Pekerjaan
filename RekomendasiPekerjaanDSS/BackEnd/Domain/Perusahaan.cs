namespace BackEnd.Domain;

public class Perusahaan
{
    public int PerusahaanId { get; set; }
    public string NamaPerusahaan { get; set; } = string.Empty;
    public string? PosisiTersedia { get; set; }
    public string? Lokasi { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<NilaiPerusahaan> Nilai { get; set; } = new List<NilaiPerusahaan>();
}