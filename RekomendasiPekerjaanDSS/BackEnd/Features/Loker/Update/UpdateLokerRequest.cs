namespace BackEnd.Features.Loker.Update;

public record UpdateLokerRequest(
    string? NamaPerusahaan,
    string? PosisiTersedia,
    string? Lokasi,
    List<Create.NilaiLokerInput>? NilaiPerKriteria
);