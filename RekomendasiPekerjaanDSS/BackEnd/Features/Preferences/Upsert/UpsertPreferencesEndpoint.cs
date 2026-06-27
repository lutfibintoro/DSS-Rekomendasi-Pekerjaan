using BackEnd.Common;
using BackEnd.Common.Auth;
using BackEnd.Common.Errors;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace BackEnd.Features.Preferences.Upsert;

public class UpsertPreferencesEndpoint : Endpoint<UpsertPreferencesRequest, ApiResponse<List<PreferensiDto>>>
{
    private readonly IMediator _mediator;
    private readonly CurrentUser _current;
    private readonly IValidator<UpsertPreferencesRequest> _validator;

    public UpsertPreferencesEndpoint(IMediator mediator, CurrentUser current, IValidator<UpsertPreferencesRequest> validator)
    {
        _mediator = mediator;
        _current = current;
        _validator = validator;
    }

    public override void Configure()
    {
        Post("/api/preferences");
        Summary(s => s.Summary = "Simpan / upsert preferensi user (replace strategy)");
    }

    public override async Task HandleAsync(UpsertPreferencesRequest req, CancellationToken ct)
    {
        if (!_current.IsAuthenticated || !_current.IsPelamar)
            throw new UnauthorizedAccessException("Hanya pelamar yang bisa mengisi preferensi");

        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
            throw new AppValidationException(validation.Errors);

        var cmd = new UpsertPreferencesCommand(_current.UserId, req.Preferences);
        var result = await _mediator.Send(cmd, ct);
        Response = ApiResponse<List<PreferensiDto>>.Ok(result, "Preferensi berhasil disimpan");
    }
}