using BackEnd.Common;
using BackEnd.Common.Auth;
using BackEnd.Common.Errors;
using BackEnd.Features.Loker.GetAll;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace BackEnd.Features.Loker.Update;

public class UpdateLokerEndpoint : Endpoint<UpdateLokerRequest, ApiResponse<LokerDto>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;
    private readonly IValidator<UpdateLokerRequest> _validator;

    public UpdateLokerEndpoint(IMediator mediator, CurrentUser current, IValidator<UpdateLokerRequest> validator)
    {
        _mediator = mediator;
        _current = current;
        _validator = validator;
    }

    public override void Configure()
    {
        Put("/api/loker/{perusahaanId:int}");
        Summary(s => s.Summary = "Update data loker (admin only)");
    }

    public override async Task HandleAsync(UpdateLokerRequest req, CancellationToken ct)
    {
        if (!_current.IsAdmin)
            throw new UnauthorizedAccessException("Hanya admin");

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
            throw new AppValidationException(validation.Errors);

        var id = Route<int>("perusahaanId");
        var cmd = new UpdateLokerCommand(id, req.NamaPerusahaan, req.PosisiTersedia, req.Lokasi, req.NilaiPerKriteria);
        var result = await _mediator.Send(cmd, ct);
        Response = ApiResponse<LokerDto>.Ok(result, "Loker berhasil diperbarui");
    }
}