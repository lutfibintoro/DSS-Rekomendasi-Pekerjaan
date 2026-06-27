using MediatR;

namespace BackEnd.Features.Rekomendasi;

public record GetRekomendasiQuery(int UserId) : IRequest<List<RekomendasiItem>>;