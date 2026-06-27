using BackEnd.Common;
using BackEnd.Common.Auth;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Rekomendasi;

public class GetRekomendasiEndpoint : EndpointWithoutRequest<ApiResponse<List<RekomendasiItem>>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;

    public GetRekomendasiEndpoint(IMediator mediator, CurrentUser current)
    {
        _mediator = mediator;
        _current = current;
    }

    public override void Configure()
    {
        Get("/api/rekomendasi");
        Summary(s => s.Summary = "Ambil daftar loker yang sudah diranking sesuai preferensi user (SAW)");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!_current.IsPelamar)
            throw new UnauthorizedAccessException("Hanya pelamar");

        var data = await _mediator.Send(new GetRekomendasiQuery(_current.UserId), ct);
        Response = ApiResponse<List<RekomendasiItem>>.Ok(data);
    }
}