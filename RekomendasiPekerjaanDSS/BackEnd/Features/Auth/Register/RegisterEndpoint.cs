using BackEnd.Common;
using BackEnd.Common.Errors;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace BackEnd.Features.Auth.Register;

public class RegisterEndpoint : Endpoint<RegisterRequest, ApiResponse<RegisterResult>>
{
    private readonly IMediator _mediator;
    private readonly IValidator<RegisterRequest> _validator;

    public RegisterEndpoint(IMediator mediator, IValidator<RegisterRequest> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    public override void Configure()
    {
        Post("/api/auth/register");
        AllowAnonymous();
        Summary(s => s.Summary = "Registrasi pengguna baru (pelamar/admin)");
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
            throw new AppValidationException(validation.Errors);

        var cmd = new RegisterCommand(req.Username, req.Email, req.Password, req.Role);
        var result = await _mediator.Send(cmd, ct);
        Response = ApiResponse<RegisterResult>.Ok(result, "Registrasi berhasil");
    }
}