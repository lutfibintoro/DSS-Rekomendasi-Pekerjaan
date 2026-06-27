using BackEnd.Common;
using BackEnd.Common.Errors;
using FastEndpoints;
using FluentValidation;
using MediatR;

namespace BackEnd.Features.Auth.Login;

public class LoginEndpoint : Endpoint<LoginRequest, ApiResponse<LoginResult>>
{
    private readonly IMediator _mediator;
    private readonly IValidator<LoginRequest> _validator;

    public LoginEndpoint(IMediator mediator, IValidator<LoginRequest> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
        Summary(s => s.Summary = "Login (validasi email & password, tanpa token)");
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
            throw new AppValidationException(validation.Errors);

        var cmd = new LoginCommand(req.Email, req.Password);
        var result = await _mediator.Send(cmd, ct);
        Response = ApiResponse<LoginResult>.Ok(result, "Login berhasil");
    }
}