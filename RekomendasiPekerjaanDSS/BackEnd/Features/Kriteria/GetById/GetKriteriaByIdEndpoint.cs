using BackEnd.Common;
using BackEnd.Features.Kriteria.GetAll;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Kriteria.GetById;

public class GetKriteriaByIdEndpoint : EndpointWithoutRequest<ApiResponse<KriteriaDto>>
{
    private readonly IMediator _mediator;
    public GetKriteriaByIdEndpoint(IMediator mediator) => _mediator = mediator;

    public override void Configure()
    {
        Get("/api/kriteria/{kriteriaId}");
        Summary(s => s.Summary = "Ambil detail satu kriteria");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("kriteriaId") ?? "";
        var data = await _mediator.Send(new GetKriteriaByIdQuery(id), ct);
        Response = ApiResponse<KriteriaDto>.Ok(data);
    }
}