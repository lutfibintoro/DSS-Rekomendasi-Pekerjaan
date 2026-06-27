using BackEnd.Common;
using BackEnd.Common.Auth;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Loker.Delete;

public class DeleteLokerEndpoint : EndpointWithoutRequest<ApiResponse<DeleteLokerResult>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;

    public DeleteLokerEndpoint(IMediator mediator, CurrentUser current)
    {
        _mediator = mediator;
        _current = current;
    }

    public override void Configure()
    {
        Delete("/api/loker/{perusahaanId:int}");
        Summary(s => s.Summary = "Hapus loker (admin only)");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!_current.IsAdmin)
            throw new UnauthorizedAccessException("Hanya admin");

        var id = Route<int>("perusahaanId");
        var result = await _mediator.Send(new DeleteLokerCommand(id), ct);
        Response = ApiResponse<DeleteLokerResult>.Ok(result, "Loker berhasil dihapus");
    }
}