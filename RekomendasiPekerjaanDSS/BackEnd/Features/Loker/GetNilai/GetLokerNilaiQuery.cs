using BackEnd.Features.Loker.GetAll;
using MediatR;

namespace BackEnd.Features.Loker.GetNilai;

public record GetLokerNilaiQuery(int PerusahaanId) : IRequest<List<NilaiLokerDto>>;