using BackEnd.Common;
using BackEnd.Common.Auth;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Preferences.Reset;

public class ResetPreferencesEndpoint : EndpointWithoutRequest<ApiResponse<ResetPreferencesResult>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;

    public ResetPreferencesEndpoint(IMediator mediator, CurrentUser current)
    {
        _mediator = mediator;
        _current = current;
    }

    public override void Configure()
    {
        Delete("/api/preferences");
        Summary(s => s.Summary = "Reset / hapus semua preferensi user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!_current.IsPelamar)
            throw new UnauthorizedAccessException("Hanya pelamar");

        var result = await _mediator.Send(new ResetPreferencesCommand(_current.UserId), ct);
        Response = ApiResponse<ResetPreferencesResult>.Ok(result, "Preferensi berhasil dihapus");
    }
}