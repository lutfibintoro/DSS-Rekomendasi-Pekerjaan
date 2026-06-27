using MediatR;

namespace BackEnd.Features.Auth.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string Role
) : IRequest<RegisterResult>;

public record RegisterResult(int UserId, string Username, string Email, string Role);