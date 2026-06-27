using BackEnd.Features.Loker.GetAll;
using MediatR;

namespace BackEnd.Features.Loker.Create;

public record CreateLokerCommand(
    string NamaPerusahaan,
    string PosisiTersedia,
    string Lokasi,
    List<NilaiLokerInput> NilaiPerKriteria
) : IRequest<LokerDto>;