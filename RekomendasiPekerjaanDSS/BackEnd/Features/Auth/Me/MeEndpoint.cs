using BackEnd.Common;
using BackEnd.Common.Auth;
using FastEndpoints;

namespace BackEnd.Features.Auth.Me;

public record MeResponse(int UserId, string Username, string Email, string Role);

public class MeEndpoint : EndpointWithoutRequest<ApiResponse<MeResponse>>
{
    private readonly CurrentUser _current;
    public MeEndpoint(CurrentUser current) => _current = current;

    public override void Configure()
    {
        Get("/api/auth/me");
        Summary(s => s.Summary = "Ambil profil user yang sedang login");
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        if (!_current.IsAuthenticated)
            throw new UnauthorizedAccessException("Belum login");

        Response = ApiResponse<MeResponse>.Ok(new MeResponse(
            _current.UserId, _current.Username, _current.Email, _current.Role));
        return Task.CompletedTask;
    }
}