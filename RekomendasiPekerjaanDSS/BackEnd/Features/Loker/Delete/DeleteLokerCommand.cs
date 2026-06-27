using MediatR;

namespace BackEnd.Features.Loker.Delete;

public record DeleteLokerCommand(int PerusahaanId) : IRequest<DeleteLokerResult>;

public record DeleteLokerResult(int DeletedId);