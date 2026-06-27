using BackEnd.Common;
using BackEnd.Common.Auth;
using BackEnd.Common.Errors;
using BackEnd.Features.Loker.GetAll;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace BackEnd.Features.Loker.Create;

public class CreateLokerEndpoint : Endpoint<CreateLokerRequest, ApiResponse<LokerDto>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;
    private readonly IValidator<CreateLokerRequest> _validator;

    public CreateLokerEndpoint(IMediator mediator, CurrentUser current, IValidator<CreateLokerRequest> validator)
    {
        _mediator = mediator;
        _current = current;
        _validator = validator;
    }

    public override void Configure()
    {
        Post("/api/loker");
        Summary(s => s.Summary = "Tambah loker baru (admin only)");
    }

    public override async Task HandleAsync(CreateLokerRequest req, CancellationToken ct)
    {
        if (!_current.IsAdmin)
            throw new UnauthorizedAccessException("Hanya admin");

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
            throw new AppValidationException(validation.Errors);

        var cmd = new CreateLokerCommand(req.NamaPerusahaan, req.PosisiTersedia, req.Lokasi, req.NilaiPerKriteria);
        var result = await _mediator.Send(cmd, ct);
        Response = ApiResponse<LokerDto>.Ok(result, "Loker berhasil dibuat");
    }
}