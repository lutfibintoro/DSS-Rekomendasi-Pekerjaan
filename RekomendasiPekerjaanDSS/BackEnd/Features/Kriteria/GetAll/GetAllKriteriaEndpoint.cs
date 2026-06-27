using BackEnd.Common;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Kriteria.GetAll;

public class GetAllKriteriaEndpoint : EndpointWithoutRequest<ApiResponse<List<KriteriaDto>>>
{
    private readonly IMediator _mediator;
    public GetAllKriteriaEndpoint(IMediator mediator) => _mediator = mediator;

    public override void Configure()
    {
        Get("/api/kriteria");
        Summary(s => s.Summary = "Ambil semua kriteria (C1 - C10) dengan deskripsi skalanya");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var data = await _mediator.Send(new GetAllKriteriaQuery(), ct);
        Response = ApiResponse<List<KriteriaDto>>.Ok(data);
    }
}