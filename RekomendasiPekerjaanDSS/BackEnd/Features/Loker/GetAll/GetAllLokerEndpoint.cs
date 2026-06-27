using BackEnd.Common;
using BackEnd.Common.Auth;
using FastEndpoints;
using MediatR;

namespace BackEnd.Features.Loker.GetAll;

public class GetAllLokerEndpoint : EndpointWithoutRequest<ApiResponse<List<LokerDto>>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;

    public GetAllLokerEndpoint(IMediator mediator, CurrentUser current)
    {
        _mediator = mediator;
        _current = current;
    }

    public override void Configure()
    {
        Get("/api/loker");
        Summary(s => s.Summary = "Ambil semua daftar loker (admin)");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (!_current.IsAdmin)
            throw new UnauthorizedAccessException("Hanya admin");

        var data = await _mediator.Send(new GetAllLokerQuery(), ct);
        Response = ApiResponse<List<LokerDto>>.Ok(data);
    }
}