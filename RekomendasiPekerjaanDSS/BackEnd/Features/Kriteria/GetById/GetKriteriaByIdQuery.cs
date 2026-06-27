using BackEnd.Features.Kriteria.GetAll;
using MediatR;

namespace BackEnd.Features.Kriteria.GetById;

public record GetKriteriaByIdQuery(string KriteriaId) : IRequest<KriteriaDto>;