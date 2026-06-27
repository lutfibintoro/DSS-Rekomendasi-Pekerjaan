using MediatR;

namespace BackEnd.Features.Loker.GetAll;

public record NilaiLokerDto(int NilaiId, int PerusahaanId, string KriteriaId, string NilaiRiil, short NilaiSkala);

public record LokerDto(
    int PerusahaanId,
    string NamaPerusahaan,
    string? PosisiTersedia,
    string? Lokasi,
    List<NilaiLokerDto> Nilai
);

public record GetAllLokerQuery() : IRequest<List<LokerDto>>;