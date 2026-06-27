namespace BackEnd.Domain;

public class PreferensiUser
{
    public int PreferenceId { get; set; }
    public int UserId { get; set; }
    public string KriteriaId { get; set; } = string.Empty;
    public string NilaiAsli { get; set; } = string.Empty;
    public short NilaiBobot { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Kriteria? Kriteria { get; set; }
}