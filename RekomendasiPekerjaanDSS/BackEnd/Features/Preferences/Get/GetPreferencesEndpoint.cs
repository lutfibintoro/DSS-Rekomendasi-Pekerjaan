using BackEnd.Common;
using BackEnd.Common.Auth;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Preferences.Get;

public class GetPreferencesEndpoint : EndpointWithoutRequest<ApiResponse<List<PreferensiDto>>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;

    public GetPreferencesEndpoint(IMediator mediator, CurrentUser current)
    {
        _mediator = mediator;
        _current = current;
    }

    public override void Configure()
    {
        Get("/api/preferences");
        Summary(s => s.Summary = "Ambil preferensi user yang sedang login");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!_current.IsPelamar)
            throw new UnauthorizedAccessException("Hanya pelamar");

        var data = await _mediator.Send(new GetPreferencesQuery(_current.UserId), ct);
        Response = ApiResponse<List<PreferensiDto>>.Ok(data);
    }
}