using BackEnd.Common;
using BackEnd.Features.Loker.GetAll;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Loker.GetNilai;

public class GetLokerNilaiEndpoint : EndpointWithoutRequest<ApiResponse<List<NilaiLokerDto>>>
{
    private readonly IMediator _mediator;
    public GetLokerNilaiEndpoint(IMediator mediator) => _mediator = mediator;

    public override void Configure()
    {
        Get("/api/loker/{perusahaanId:int}/nilai");
        Summary(s => s.Summary = "Ambil nilai per-kriteria untuk satu loker");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("perusahaanId");
        var data = await _mediator.Send(new GetLokerNilaiQuery(id), ct);
        Response = ApiResponse<List<NilaiLokerDto>>.Ok(data);
    }
}