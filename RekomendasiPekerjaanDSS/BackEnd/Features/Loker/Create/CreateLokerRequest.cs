namespace BackEnd.Features.Loker.Create;

public record NilaiLokerInput(string KriteriaId, string NilaiRiil);

public record CreateLokerRequest(
    string NamaPerusahaan,
    string PosisiTersedia,
    string Lokasi,
    List<NilaiLokerInput> NilaiPerKriteria
);