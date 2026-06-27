using BackEnd.Features.Loker.GetAll;
using MediatR;

namespace BackEnd.Features.Loker.GetById;

public record GetLokerByIdQuery(int PerusahaanId) : IRequest<LokerDto>;