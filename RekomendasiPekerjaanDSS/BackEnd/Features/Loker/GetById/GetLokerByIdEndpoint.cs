using BackEnd.Common;
using BackEnd.Features.Loker.GetAll;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Loker.GetById;

public class GetLokerByIdEndpoint : EndpointWithoutRequest<ApiResponse<LokerDto>>
{
    private readonly IMediator _mediator;
    public GetLokerByIdEndpoint(IMediator mediator) => _mediator = mediator;

    public override void Configure()
    {
        Get("/api/loker/{perusahaanId:int}");
        Summary(s => s.Summary = "Ambil detail satu loker");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("perusahaanId");
        var data = await _mediator.Send(new GetLokerByIdQuery(id), ct);
        Response = ApiResponse<LokerDto>.Ok(data);
    }
}