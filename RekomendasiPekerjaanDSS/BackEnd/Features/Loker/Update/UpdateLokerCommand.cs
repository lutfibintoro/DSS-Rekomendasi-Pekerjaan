using BackEnd.Features.Loker.Create;
using BackEnd.Features.Loker.GetAll;
using MediatR;

namespace BackEnd.Features.Loker.Update;

public record UpdateLokerCommand(
    int PerusahaanId,
    string? NamaPerusahaan,
    string? PosisiTersedia,
    string? Lokasi,
    List<NilaiLokerInput>? NilaiPerKriteria
) : IRequest<LokerDto>;