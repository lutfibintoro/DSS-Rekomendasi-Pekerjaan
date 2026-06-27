namespace BackEnd.Domain;

public class Kriteria
{
    public string KriteriaId { get; set; } = string.Empty;
    public string NamaKriteria { get; set; } = string.Empty;
    public string JenisAtribut { get; set; } = "benefit";
    public string? Satuan { get; set; }
    public string? Deskripsi { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}